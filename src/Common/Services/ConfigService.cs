using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Services
{
    public class ConfigService
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly string[] cities = new string[]
        {
          "gothenburg",
          "nottingham"
        };

        private readonly Dictionary<string, Map> maps = new Dictionary<string, Map>();
        private readonly Dictionary<PersonalityKey, PersonalitySpecification> personalities = new Dictionary<PersonalityKey, PersonalitySpecification>();
        private readonly Dictionary<AwardKey, AwardSpecification> awards = new Dictionary<AwardKey, AwardSpecification>();

        public ConfigService()
        {
            this.SetMaps();
            this.SetAwards();
            this.SetPersonalities();
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

        private void SetAwards()
        {
            foreach (string city in this.cities)
            {
                var filename = Path.Join(Environment.CurrentDirectory, "Config", $"LocalHost.Config.{city}.awards.json");
                var fileContent = File.ReadAllText(filename);

                Awards awards = JsonSerializer.Deserialize<Awards>(fileContent, this.jsonSerializerOptions);
                if ((object)awards == null)
                    throw new Exception("Count not parse awards.json");
                foreach (KeyValuePair<AwardType, AwardSpecification> awardSpecification in awards.AwardSpecifications)
                    this.awards.Add(new AwardKey(city, awardSpecification.Key), awardSpecification.Value);
            }
        }

        private void SetPersonalities()
        {
            foreach (string city in this.cities)
            {
                var filename = Path.Join(Environment.CurrentDirectory, "Config", $"LocalHost.Config.{city}.awards.json");
                var fileContent = File.ReadAllText(filename);

                Personalities personalities = JsonSerializer.Deserialize<Personalities>(fileContent, this.jsonSerializerOptions);
                if ((object)personalities == null)
                    throw new Exception("Count not parse personalities.json");
                foreach (KeyValuePair<Personality, PersonalitySpecification> personalitySpecification in personalities.PersonalitySpecifications)
                    this.personalities.Add(new PersonalityKey(city, personalitySpecification.Key), personalitySpecification.Value);
            }
        }

        public Map? GetMap(string mapName)
        {
            Map map = this.maps.GetValueOrDefault<string, Map>(mapName);
            if ((object)map == null)
                throw new Exception("Unknown map:" + mapName);

            return map with
            {
                Customers = map.Customers.Select((Customer x) => x with
                {
                    Loan = x.Loan with { }
                }).ToList()
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
