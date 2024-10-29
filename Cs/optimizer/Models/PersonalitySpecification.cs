
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record PersonalitySpecification()
    {
        public Decimal HappinessMultiplier { get; set; }

        public Decimal? AcceptedMinInterest { get; set; }

        public Decimal? AcceptedMaxInterest { get; set; }

        public Decimal LivingStandardMultiplier { get; set; }
    }
}
