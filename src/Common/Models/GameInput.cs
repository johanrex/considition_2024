
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
