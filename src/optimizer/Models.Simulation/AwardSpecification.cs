using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Models.Simulation
{
    public record AwardSpecification
    {
        public double Cost { get; init; }
        public double BaseHappiness { get; set; }
    }
}
