// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.GameInput
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
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
