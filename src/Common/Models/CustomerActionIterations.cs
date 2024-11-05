// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.CustomerActionIteration
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace Common.Models
{
    [JsonConverter(typeof(CustomerActionIterationConverter))]
    public record CustomerActionIteration()
    {
        public Dictionary<string, CustomerAction> CustomerActions;
    }
}
