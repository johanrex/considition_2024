// Decompiled with JetBrains decompiler
// Type: LocalHost.JsonConverterUtils
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

#nullable enable
namespace Common
{
    public class JsonConverterUtils
    {
        public static void ReadStartArray(ref Utf8JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Start array expected");
        }

        public static void ReadEndArray(ref Utf8JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException("End array expected");
        }

        public static void ReadStartObject(ref Utf8JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Start object expected");
        }

        public static void ReadEndObject(ref Utf8JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException("End object expected");
        }

        public static (string Key, T? Value) GetPropertyKvp<T>(ref Utf8JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Property name expected");
            }
            string name = reader.GetString();
            if (name == null)
            {
                throw new JsonException("Property name cannot be null");
            }
            reader.Read();
            return (Key: name, Value: (dynamic)(reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetDouble(),
                JsonTokenType.False => false,
                JsonTokenType.True => true,
                JsonTokenType.Null => null,
                _ => throw new JsonException("Property value must be a string, number, boolean, or null"),
            }));
        }
    }
}
