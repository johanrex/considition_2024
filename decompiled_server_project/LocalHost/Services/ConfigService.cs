// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.ConfigService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1678F578-689D-4062-BED4-DD7ABDE09D6A
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Interfaces;
using LocalHost.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

#nullable enable
namespace LocalHost.Services
{
    public class ConfigService : IConfigService
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        private Dictionary<string, Map> maps = new Dictionary<string, Map>();

        public Dictionary<AwardType, AwardSpecification> Awards { get; private set; } = new Dictionary<AwardType, AwardSpecification>();

        public Dictionary<Personality, PersonalitySpecification> Personalities { get; private set; } = new Dictionary<Personality, PersonalitySpecification>();

        public ConfigService()
        {
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
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName + ".Config.awards.json"))
            {
                using (StreamReader streamReader = new StreamReader(manifestResourceStream, Encoding.UTF8))
                    this.Awards = JsonSerializer.Deserialize<LocalHost.Models.Awards>(streamReader.ReadToEnd(), this.jsonSerializerOptions)?.AwardSpecifications ?? throw new Exception("Count not parse awards.json");
            }
        }

        private void SetPersonalities(string assemblyName)
        {
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName + ".Config.personalities.json"))
            {
                using (StreamReader streamReader = new StreamReader(manifestResourceStream, Encoding.UTF8))
                    this.Personalities = JsonSerializer.Deserialize<LocalHost.Models.Personalities>(streamReader.ReadToEnd(), this.jsonSerializerOptions)?.PersonalitySpecifications ?? throw new Exception("Count not parse personalities.json");
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
                    Loan = x.Loan.\u003CClone\u003E\u0024()
                })).ToList<Customer>()
            };
        }
    }
}
