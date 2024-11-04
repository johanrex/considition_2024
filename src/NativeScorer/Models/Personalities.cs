// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Personalities
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace NativeScorer.Models
{
    public record Personalities()
    {
        [JsonPropertyName("Personalities")]
        [JsonConverter(typeof(PersonalitySpecificationsConverter))]
        public Dictionary<Personality, PersonalitySpecification> PersonalitySpecifications { get; set; }
    }
}
