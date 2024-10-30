// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResult
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1790A9F3-C8FD-4294-9282-EE084D3CC633
// Assembly location: C:\temp\app\LocalHost.dll

using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record GameResult
    {
        public double TotalProfit { get; init; }

        public double HappynessScore { get; init; }

        public double EnvironmentalImpact { get; init; }

        public double TotalScore => this.TotalProfit + this.HappynessScore + this.EnvironmentalImpact;

        public string MapName { get; set; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("TotalProfit = ");
            builder.Append(this.TotalProfit.ToString());
            builder.Append(", HappynessScore = ");
            builder.Append(this.HappynessScore.ToString());
            builder.Append(", EnvironmentalImpact = ");
            builder.Append(this.EnvironmentalImpact.ToString());
            builder.Append(", TotalScore = ");
            builder.Append(this.TotalScore.ToString());
            builder.Append(", MapName = ");
            builder.Append((object)this.MapName);
            return true;
        }

        public GameResult()
        {
        }
    }
}
