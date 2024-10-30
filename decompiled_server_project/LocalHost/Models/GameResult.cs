// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResult
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record GameResult
    {
        public Decimal TotalProfit { get; init; }

        public Decimal HappynessScore { get; init; }

        public Decimal EnvironmentalImpact { get; init; }

        public Decimal TotalScore => this.TotalProfit + this.HappynessScore + this.EnvironmentalImpact;

        public string MapName { get; set; }

        public List<string> UnlockedAchievements { get; init; }

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
            builder.Append(", UnlockedAchievements = ");
            builder.Append((object)this.UnlockedAchievements);
            return true;
        }

        public GameResult()
        {
        }
    }
}
