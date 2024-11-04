// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameResponse
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0E63F0E5-C165-45D8-B0A0-7E9412810D3A
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace optimizer.Models.Simulation
{
    public record GameResponse
    {
        public Guid? GameId { get; init; }

        public GameResult? Score { get; init; }

        public string? Message { get; set; }
    }
}
