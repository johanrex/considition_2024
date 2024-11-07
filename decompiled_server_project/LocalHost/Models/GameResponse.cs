﻿// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResponse
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D1B7BF3C-328E-422C-8A9F-0E1266BF8FE0
// Assembly location: C:\temp\app\LocalHost.dll

using System;
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
            return true;
        }

        public GameResponse()
        {
        }
    }
}
