// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResult
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0E63F0E5-C165-45D8-B0A0-7E9412810D3A
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace optimizer.Models
{
    public record GameResult
    {
        public decimal TotalProfit { get; init; }

        public decimal HappynessScore { get; init; }

        public decimal EnvironmentalImpact { get; init; }

        public decimal TotalScore => TotalProfit + HappynessScore + EnvironmentalImpact;

        public required string MapName { get; set; }

        public required List<string> UnlockedAchievements { get; init; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("TotalProfit = ");
            builder.Append(TotalProfit.ToString());
            builder.Append(", HappynessScore = ");
            builder.Append(HappynessScore.ToString());
            builder.Append(", EnvironmentalImpact = ");
            builder.Append(EnvironmentalImpact.ToString());
            builder.Append(", TotalScore = ");
            builder.Append(TotalScore.ToString());
            builder.Append(", MapName = ");
            builder.Append((object)MapName);
            builder.Append(", UnlockedAchievements = ");
            builder.Append(UnlockedAchievements);
            return true;
        }

    }
}
