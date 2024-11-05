// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResult
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79D8B4B1-4F4D-4A0C-BFF7-A27C4AB10C69
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace Common.Models
{
    public record GameResult
    {
        public required string MapName { get; init; }

        public long TotalProfit { get; init; }

        public long HappinessScore { get; init; }

        public long EnvironmentalImpact { get; init; }

        public long TotalScore => this.TotalProfit + this.HappinessScore + this.EnvironmentalImpact;
    }
}
