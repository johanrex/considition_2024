// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.CustomerActionIteration
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Models
{
    [JsonConverter(typeof(CustomerActionIterationConverter))]
    public record CustomerActionIteration()
    {
        public Dictionary<string, CustomerAction> CustomerActions;

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("CustomerActions = ");
            builder.Append((object)this.CustomerActions);
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            return EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Dictionary<string, CustomerAction>>.Default.GetHashCode(this.CustomerActions);
        }

        [CompilerGenerated]
        public virtual bool Equals(CustomerActionIteration? other)
        {
            if ((object)this == (object)other)
                return true;
            return (object)other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Dictionary<string, CustomerAction>>.Default.Equals(this.CustomerActions, other.CustomerActions);
        }

        [CompilerGenerated]
        protected CustomerActionIteration(CustomerActionIteration original)
        {
            this.CustomerActions = original.CustomerActions;
        }
    }
}
