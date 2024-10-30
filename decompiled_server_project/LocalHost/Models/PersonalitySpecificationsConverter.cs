// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.PersonalitySpecificationsConverter
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Models
{
    public class PersonalitySpecificationsConverter :
      JsonConverter<Dictionary<Personality, PersonalitySpecification>>
    {
        public override Dictionary<Personality, PersonalitySpecification>? Read(
          ref Utf8JsonReader reader,
          Type typeToConvert,
          JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Start object expected");
            Dictionary<Personality, PersonalitySpecification> dictionary = new Dictionary<Personality, PersonalitySpecification>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return dictionary;
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Property name expected");
                string str = reader.GetString();
                Personality result;
                if (!Enum.TryParse<Personality>(str, true, out result))
                    throw new JsonException("Invalid personality name: " + str);
                PersonalitySpecification personalitySpecification = PersonalitySpecificationsConverter.ReadPersonalitySpec(ref reader);
                dictionary.Add(result, personalitySpecification);
            }
            throw new JsonException();
        }

        private static PersonalitySpecification ReadPersonalitySpec(ref Utf8JsonReader reader)
        {
            JsonConverterUtils.ReadStartObject(ref reader);
            (string Key, Decimal? Value) propertyKvp1 = JsonConverterUtils.GetPropertyKvp<Decimal?>(ref reader);
            (string Key, Decimal? Value) propertyKvp2 = JsonConverterUtils.GetPropertyKvp<Decimal?>(ref reader);
            (string Key, Decimal? Value) propertyKvp3 = JsonConverterUtils.GetPropertyKvp<Decimal?>(ref reader);
            (string Key, Decimal? Value) propertyKvp4 = JsonConverterUtils.GetPropertyKvp<Decimal?>(ref reader);
            JsonConverterUtils.ReadEndObject(ref reader);
            Dictionary<string, Decimal?> dictionary = new Dictionary<string, Decimal?>()
      {
        {
          propertyKvp1.Key,
          propertyKvp1.Value
        },
        {
          propertyKvp2.Key,
          propertyKvp2.Value
        },
        {
          propertyKvp3.Key,
          propertyKvp3.Value
        },
        {
          propertyKvp4.Key,
          propertyKvp4.Value
        }
      };
            return new PersonalitySpecification()
            {
                HappinessMultiplier = dictionary.GetValueOrDefault<string, Decimal?>("happinessMultiplier", new Decimal?(1.0M)).Value,
                LivingStandardMultiplier = dictionary.GetValueOrDefault<string, Decimal?>("livingStandardMultiplier", new Decimal?(1.0M)).Value,
                AcceptedMinInterest = dictionary.GetValueOrDefault<string, Decimal?>("acceptedMinInterest", new Decimal?(0.00001M)),
                AcceptedMaxInterest = dictionary.GetValueOrDefault<string, Decimal?>("acceptedMaxInterest", new Decimal?(Decimal.MaxValue))
            };
        }

        public override void Write(
          Utf8JsonWriter writer,
          Dictionary<Personality, PersonalitySpecification> value,
          JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
