// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.CustomerActionIterationConverter
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D1B7BF3C-328E-422C-8A9F-0E1266BF8FE0
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Models
{
    public class CustomerActionIterationConverter : JsonConverter<CustomerActionIteration>
    {
        public override CustomerActionIteration? Read(
          ref Utf8JsonReader reader,
          Type typeToConvert,
          JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Start object expected");
            Dictionary<string, CustomerAction> dictionary = new Dictionary<string, CustomerAction>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return new CustomerActionIteration()
                    {
                        CustomerActions = dictionary
                    };
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Property name expected");
                string key = reader.GetString();
                if (key == null)
                    throw new JsonException("Customer name cannot be null");
                CustomerAction customerAction = CustomerActionIterationConverter.GetCustomerAction(ref reader);
                dictionary.Add(key, customerAction);
            }
            throw new JsonException();
        }

        private static CustomerAction GetCustomerAction(ref Utf8JsonReader reader)
        {
            JsonConverterUtils.ReadStartObject(ref reader);
            (string Key, string Value) propertyKvp1 = JsonConverterUtils.GetPropertyKvp<string>(ref reader);
            (string Key, string Value) propertyKvp2 = JsonConverterUtils.GetPropertyKvp<string>(ref reader);
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
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
            JsonConverterUtils.ReadEndObject(ref reader);
            CustomerAction customerAction1 = new CustomerAction();
            CustomerAction customerAction2 = customerAction1;
            CustomerActionType customerActionType;
            switch (dictionary["Type"])
            {
                case "Skip":
                    customerActionType = CustomerActionType.Skip;
                    break;
                case "Award":
                    customerActionType = CustomerActionType.Award;
                    break;
                case null:
                    customerActionType = CustomerActionType.Skip;
                    break;
                default:
                    throw new JsonException("Invalid customer action type: " + dictionary["Type"]);
            }
            customerAction2.Type = customerActionType;
            CustomerAction customerAction3 = customerAction1;
            string str = dictionary["Award"];
            AwardType awardType;
            if (str != null)
            {
                switch (str.Length)
                {
                    case 4:
                        if (str == "None")
                        {
                            awardType = AwardType.None;
                            goto label_25;
                        }
                        else
                            break;
                    case 8:
                        if (str == "GiftCard")
                        {
                            awardType = AwardType.GiftCard;
                            goto label_25;
                        }
                        else
                            break;
                    case 9:
                        if (str == "IkeaCheck")
                        {
                            awardType = AwardType.IkeaCheck;
                            goto label_25;
                        }
                        else
                            break;
                    case 14:
                        switch (str[0])
                        {
                            case 'I':
                                if (str == "IkeaFoodCoupon")
                                {
                                    awardType = AwardType.IkeaFoodCoupon;
                                    goto label_25;
                                }
                                else
                                    break;
                            case 'N':
                                if (str == "NoInterestRate")
                                {
                                    awardType = AwardType.NoInterestRate;
                                    goto label_25;
                                }
                                else
                                    break;
                        }
                        break;
                    case 16:
                        if (str == "HalfInterestRate")
                        {
                            awardType = AwardType.HalfInterestRate;
                            goto label_25;
                        }
                        else
                            break;
                    case 17:
                        if (str == "IkeaDeliveryCheck")
                        {
                            awardType = AwardType.IkeaDeliveryCheck;
                            goto label_25;
                        }
                        else
                            break;
                }
            }
            if (str != null)
                throw new JsonException("Invalid award type: " + dictionary["Award"]);
            awardType = AwardType.None;
        label_25:
            customerAction3.Award = awardType;
            return customerAction1;
        }

        public override void Write(
          Utf8JsonWriter writer,
          CustomerActionIteration value,
          JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach ((string str, CustomerAction customerAction) in value.CustomerActions)
            {
                writer.WritePropertyName(str);
                writer.WriteStartObject();
                writer.WriteString("Type", customerAction.Type.ToString());
                writer.WriteString("Award", customerAction.Award.ToString());
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }
    }
}
