// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.AwardSpecification
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37D09AE0-70E5-46F8-B3D7-80D789257673
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
