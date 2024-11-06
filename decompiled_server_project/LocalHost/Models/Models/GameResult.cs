// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResult
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E185BA7-B99B-4FD6-9E2E-A742DD973CDE
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    [RequiredMember]
    public record GameResult
    {
        [RequiredMember]
        public string MapName { get; init; }

        public long TotalProfit { get; init; }

        public long HappinessScore { get; init; }

        public long EnvironmentalImpact { get; init; }

        public long TotalScore => this.TotalProfit + this.HappinessScore + this.EnvironmentalImpact;

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("MapName = ");
            builder.Append((object)this.MapName);
            builder.Append(", TotalProfit = ");
            builder.Append(this.TotalProfit.ToString());
            builder.Append(", HappinessScore = ");
            builder.Append(this.HappinessScore.ToString());
            builder.Append(", EnvironmentalImpact = ");
            builder.Append(this.EnvironmentalImpact.ToString());
            builder.Append(", TotalScore = ");
            builder.Append(this.TotalScore.ToString());
            return true;
        }

        [Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
        [CompilerFeatureRequired("RequiredMembers")]
        public GameResult()
        {
        }
    }
}
