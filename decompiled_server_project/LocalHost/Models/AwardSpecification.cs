// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.AwardSpecification
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record AwardSpecification
    {
        public Decimal Cost { get; init; }

        public Decimal BaseHappiness { get; set; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Cost = ");
            builder.Append(this.Cost.ToString());
            builder.Append(", BaseHappiness = ");
            builder.Append(this.BaseHappiness.ToString());
            return true;
        }

        public AwardSpecification()
        {
        }
    }
}
