// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResponse
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1790A9F3-C8FD-4294-9282-EE084D3CC633
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record GameResponse
    {
        public Guid? GameId { get; init; }

        public GameResult? Score { get; init; }

        public string? Message { get; set; }

        public List<string> AchievementsUnlocked { get; init; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("GameId = ");
            builder.Append(this.GameId.ToString());
            builder.Append(", Score = ");
            builder.Append((object)this.Score);
            builder.Append(", Message = ");
            builder.Append((object)this.Message);
            builder.Append(", AchievementsUnlocked = ");
            builder.Append((object)this.AchievementsUnlocked);
            return true;
        }

        public GameResponse()
        {
        }
    }
}
