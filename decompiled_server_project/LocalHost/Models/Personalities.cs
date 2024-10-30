// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Personalities
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37D09AE0-70E5-46F8-B3D7-80D789257673
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Models
{
    public record Personalities()
    {
        [JsonPropertyName("Personalities")]
        [JsonConverter(typeof(PersonalitySpecificationsConverter))]
        public Dictionary<Personality, PersonalitySpecification> PersonalitySpecifications { get; set; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("PersonalitySpecifications = ");
            builder.Append((object)this.PersonalitySpecifications);
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            // ISSUE: reference to a compiler-generated field
            return EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Dictionary<Personality, PersonalitySpecification>>.Default.GetHashCode(this.\u003CPersonalitySpecifications\u003Ek__BackingField);
        }

        [CompilerGenerated]
        public virtual bool Equals(Personalities? other)
        {
            if ((object)this == (object)other)
                return true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (object)other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Dictionary<Personality, PersonalitySpecification>>.Default.Equals(this.\u003CPersonalitySpecifications\u003Ek__BackingField, other.\u003CPersonalitySpecifications\u003Ek__BackingField);
        }

        [CompilerGenerated]
        protected Personalities(Personalities original)
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CPersonalitySpecifications\u003Ek__BackingField = original.\u003CPersonalitySpecifications\u003Ek__BackingField;
        }
    }
}
