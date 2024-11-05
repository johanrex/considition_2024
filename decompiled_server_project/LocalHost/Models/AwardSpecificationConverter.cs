// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.AwardSpecificationConverter
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79D8B4B1-4F4D-4A0C-BFF7-A27C4AB10C69
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Models
{
    public class AwardSpecificationConverter : JsonConverter<Dictionary<AwardType, AwardSpecification>>
    {
        public override Dictionary<AwardType, AwardSpecification>? Read(
          ref Utf8JsonReader reader,
          Type typeToConvert,
          JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Start object expected");
            Dictionary<AwardType, AwardSpecification> dictionary = new Dictionary<AwardType, AwardSpecification>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return dictionary;
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Property name expected");
                string str = reader.GetString();
                AwardType result;
                if (!Enum.TryParse<AwardType>(str, true, out result))
                    throw new JsonException("Invalid personality name: " + str);
                AwardSpecification awardSpecification = AwardSpecificationConverter.ReadPersonalitySpec(ref reader);
                dictionary.Add(result, awardSpecification);
            }
            throw new JsonException();
        }

        private static AwardSpecification ReadPersonalitySpec(ref Utf8JsonReader reader)
        {
            JsonConverterUtils.ReadStartObject(ref reader);
            (string Key, double Value) propertyKvp1 = JsonConverterUtils.GetPropertyKvp<double>(ref reader);
            (string Key, double Value) propertyKvp2 = JsonConverterUtils.GetPropertyKvp<double>(ref reader);
            JsonConverterUtils.ReadEndObject(ref reader);
            Dictionary<string, double> dictionary = new Dictionary<string, double>()
      {
        {
          propertyKvp1.Key,
          propertyKvp1.Value
        },
        {
          propertyKvp2.Key,
          propertyKvp2.Value
        }
      };
            return new AwardSpecification()
            {
                Cost = dictionary["cost"],
                BaseHappiness = dictionary["baseHappiness"]
            };
        }

        public override void Write(
          Utf8JsonWriter writer,
          Dictionary<AwardType, AwardSpecification> value,
          JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
