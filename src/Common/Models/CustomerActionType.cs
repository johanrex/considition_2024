﻿// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.CustomerActionType
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

#nullable disable
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomerActionType
    {
        Skip,
        Award,
    }
}
