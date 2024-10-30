﻿// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.AwardSpecification
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1790A9F3-C8FD-4294-9282-EE084D3CC633
// Assembly location: C:\temp\app\LocalHost.dll

using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record AwardSpecification
    {
        public double Cost { get; init; }

        public double BaseHappiness { get; set; }

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
