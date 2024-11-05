// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameInput
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace Common.Models
{
    public record GameInput
    {
        public required string MapName { get; init; }

        public required List<CustomerLoanRequestProposal> Proposals { get; init; }

        public required List<CustomerActionIteration> Iterations { get; init; }
    }
}
