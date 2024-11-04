// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Awards
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1678F578-689D-4062-BED4-DD7ABDE09D6A
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Models
{
    public record Awards()
    {
        [JsonPropertyName("Awards")]
        [JsonConverter(typeof(AwardSpecificationConverter))]
        public Dictionary<AwardType, AwardSpecification> AwardSpecifications { get; set; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("AwardSpecifications = ");
            builder.Append((object)this.AwardSpecifications);
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            // ISSUE: reference to a compiler-generated field
            return EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Dictionary<AwardType, AwardSpecification>>.Default.GetHashCode(this.\u003CAwardSpecifications\u003Ek__BackingField);
        }

        [CompilerGenerated]
        public virtual bool Equals(Awards? other)
        {
            if ((object)this == (object)other)
                return true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (object)other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Dictionary<AwardType, AwardSpecification>>.Default.Equals(this.\u003CAwardSpecifications\u003Ek__BackingField, other.\u003CAwardSpecifications\u003Ek__BackingField);
        }

        [CompilerGenerated]
        protected Awards(Awards original)
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CAwardSpecifications\u003Ek__BackingField = original.\u003CAwardSpecifications\u003Ek__BackingField;
        }
    }
}
