// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.AwardSpecification
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E185BA7-B99B-4FD6-9E2E-A742DD973CDE
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
