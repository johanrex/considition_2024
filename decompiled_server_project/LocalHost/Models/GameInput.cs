// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameInput
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace LocalHost.Models
{
    [RequiredMember]
    public record GameInput
    {
        [RequiredMember]
        public string MapName { get; init; }

        [RequiredMember]
        public List<CustomerLoanRequestProposal> Proposals { get; init; }

        [RequiredMember]
        public List<CustomerActionIteration> Iterations { get; init; }

        [Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
        [CompilerFeatureRequired("RequiredMembers")]
        public GameInput()
        {
        }
    }
}
