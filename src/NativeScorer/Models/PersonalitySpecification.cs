
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace NativeScorer.Models
{
    public record PersonalitySpecification()
    {
        public double HappinessMultiplier { get; set; }

        public double? AcceptedMinInterest { get; set; }

        public double? AcceptedMaxInterest { get; set; }

        public double LivingStandardMultiplier { get; set; }

    }
}
