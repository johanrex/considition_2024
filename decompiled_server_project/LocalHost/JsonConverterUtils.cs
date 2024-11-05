// Decompiled with JetBrains decompiler
// Type: LocalHost.JsonConverterUtils
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79D8B4B1-4F4D-4A0C-BFF7-A27C4AB10C69
// Assembly location: C:\temp\app\LocalHost.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

#nullable enable
namespace LocalHost
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
                throw new JsonException("Property name expected");
            string str1 = reader.GetString();
            if (str1 == null)
                throw new JsonException("Property name cannot be null");
            reader.Read();
            object obj1;
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    obj1 = (object)reader.GetString();
                    break;
                case JsonTokenType.Number:
                    obj1 = (object)reader.GetDouble();
                    break;
                case JsonTokenType.True:
                    obj1 = (object)true;
                    break;
                case JsonTokenType.False:
                    obj1 = (object)false;
                    break;
                case JsonTokenType.Null:
                    obj1 = (object)null;
                    break;
                default:
                    throw new JsonException("Property value must be a string, number, boolean, or null");
            }
            object obj2 = obj1;
            string str2 = str1;
            // ISSUE: reference to a compiler-generated field
            if (JsonConverterUtils.\u003C\u003Eo__4<T>.\u003C\u003Ep__0 == null)
      {
                // ISSUE: reference to a compiler-generated field
                JsonConverterUtils.\u003C\u003Eo__4<T>.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, T>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(T), typeof(JsonConverterUtils)));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            T obj3 = JsonConverterUtils.\u003C\u003Eo__4<T>.\u003C\u003Ep__0.Target((CallSite)JsonConverterUtils.\u003C\u003Eo__4<T>.\u003C\u003Ep__0, obj2);
            return (str2, obj3);
        }
    }
}
