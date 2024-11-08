// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.ConfigService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 77EDA3FC-B32E-487F-8161-20E228F5089F
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Interfaces;
using LocalHost.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Services
{
    public class ConfigService : IConfigService
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly string[] cities;
        private readonly Dictionary<string, Map> maps;
        private readonly Dictionary<PersonalityKey, PersonalitySpecification> personalities;
        private readonly Dictionary<AwardKey, AwardSpecification> awards;

        public ConfigService()
        {
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add((JsonConverter)new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            this.jsonSerializerOptions = serializerOptions;
            this.cities = new string[3]
            {
        "gothenburg",
        "nottingham",
        "almhult"
            };
            this.maps = new Dictionary<string, Map>();
            this.personalities = new Dictionary<PersonalityKey, PersonalitySpecification>();
            this.awards = new Dictionary<AwardKey, AwardSpecification>();
            // ISSUE: explicit constructor call
            base.\u002Ector();
            this.SetMaps();
            string name = (Assembly.GetExecutingAssembly().GetName() ?? throw new Exception("Couldn't get assembly info.")).Name;
            if (name == null)
                throw new Exception("Couldn't get assembly name.");
            this.SetAwards(name);
            this.SetPersonalities(name);
        }

        private void SetMaps()
        {
            string str = Path.Join(Environment.CurrentDirectory, "Config");
            string[] strArray = Directory.Exists(str) ? Directory.GetFiles(Path.Join(str, "Maps")) : throw new Exception("Couldn't find 'Config' directory.");
            if (strArray.Length == 0)
                throw new Exception("No 'Maps' available in directory.");
            foreach (string path in strArray)
            {
                if (Path.GetFileName(path).StartsWith("map", StringComparison.InvariantCultureIgnoreCase))
                {
                    Map map = JsonSerializer.Deserialize<Map>(File.ReadAllText(path), this.jsonSerializerOptions);
                    if ((object)map != null)
                        this.maps.Add(map.Name, map);
                }
            }
        }

        private void SetAwards(string assemblyName)
        {
            foreach (string city in this.cities)
            {
                using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName + ".Config." + city + ".awards.json"))
                {
                    using (StreamReader streamReader = new StreamReader(manifestResourceStream, Encoding.UTF8))
                    {
                        Awards awards = JsonSerializer.Deserialize<Awards>(streamReader.ReadToEnd(), this.jsonSerializerOptions);
                        if ((object)awards == null)
                            throw new Exception("Count not parse awards.json");
                        foreach (KeyValuePair<AwardType, AwardSpecification> awardSpecification in awards.AwardSpecifications)
                            this.awards.Add(new AwardKey(city, awardSpecification.Key), awardSpecification.Value);
                    }
                }
            }
        }

        private void SetPersonalities(string assemblyName)
        {
            foreach (string city in this.cities)
            {
                using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName + ".Config." + city + ".personalities.json"))
                {
                    using (StreamReader streamReader = new StreamReader(manifestResourceStream, Encoding.UTF8))
                    {
                        Personalities personalities = JsonSerializer.Deserialize<Personalities>(streamReader.ReadToEnd(), this.jsonSerializerOptions);
                        if ((object)personalities == null)
                            throw new Exception("Count not parse personalities.json");
                        foreach (KeyValuePair<Personality, PersonalitySpecification> personalitySpecification in personalities.PersonalitySpecifications)
                            this.personalities.Add(new PersonalityKey(city, personalitySpecification.Key), personalitySpecification.Value);
                    }
                }
            }
        }

        public Map? GetMap(string mapName)
        {
            Map valueOrDefault = this.maps.GetValueOrDefault<string, Map>(mapName);
            if ((object)valueOrDefault == null)
                return (Map)null;
            return valueOrDefault with
            {
                Customers = valueOrDefault.Customers.Select<Customer, Customer>((Func<Customer, Customer>)(x => x with
                {
                    Loan = x.Loan.\u003CClone\u003E\u0024(),
                    AwardsReceived = new List<AwardType>()
                })).ToList<Customer>()
            };
        }

        public Dictionary<Personality, PersonalitySpecification> GetPersonalitySpecifications(
          string mapName)
        {
            return this.personalities.Where<KeyValuePair<PersonalityKey, PersonalitySpecification>>((Func<KeyValuePair<PersonalityKey, PersonalitySpecification>, bool>)(x => x.Key.MapName.Equals(mapName, StringComparison.InvariantCultureIgnoreCase))).Select(x => new
            {
                Key = x.Key.Personality,
                Value = x.Value
            }).ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<AwardType, AwardSpecification> GetAwardSpecifications(string mapName)
        {
            return this.awards.Where<KeyValuePair<AwardKey, AwardSpecification>>((Func<KeyValuePair<AwardKey, AwardSpecification>, bool>)(x => x.Key.MapName.Equals(mapName, StringComparison.InvariantCultureIgnoreCase))).Select(x => new
            {
                Key = x.Key.Award,
                Value = x.Value
            }).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
