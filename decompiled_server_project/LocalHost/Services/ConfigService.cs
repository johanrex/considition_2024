// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.ConfigService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37D09AE0-70E5-46F8-B3D7-80D789257673
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Interfaces;
using LocalHost.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string str = Path.Join(Environment.CurrentDirectory, "Config");
            if (!Directory.Exists(str))
                throw new Exception("\tCouldn't find 'Config' directory.");
            this.SetMaps(str);
            this.SetAwards(str);
            this.SetPersonalities(str);
        }

        private void SetMaps(string directoryPath)
        {
            string[] files = Directory.GetFiles(Path.Join(directoryPath, "Maps"));
            if (files.Length == 0)
                throw new Exception("\tNo 'Maps' available in directory.");
            foreach (string path in files)
            {
                if (Path.GetFileName(path).StartsWith("map", StringComparison.InvariantCultureIgnoreCase))
                {
                    Map map = JsonSerializer.Deserialize<Map>(File.ReadAllText(path), this.jsonSerializerOptions);
                    if ((object)map != null)
                        this.maps.Add(map.Name, map);
                }
            }
        }

        private void SetAwards(string directoryPath)
        {
            this.Awards = JsonSerializer.Deserialize<LocalHost.Models.Awards>(File.ReadAllText(Path.Join(directoryPath, "awards.json")), this.jsonSerializerOptions)?.AwardSpecifications ?? throw new Exception("Count not parse awards.json");
        }

        private void SetPersonalities(string directoryPath)
        {
            this.Personalities = JsonSerializer.Deserialize<LocalHost.Models.Personalities>(File.ReadAllText(Path.Join(directoryPath, "personalities.json")), this.jsonSerializerOptions)?.PersonalitySpecifications ?? throw new Exception("Count not parse personalities.json");
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
