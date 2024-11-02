using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal record CustomerPropositionDetails
    {
        public string CustomerName { get; set; }
        public double ScoreContribution { get; set; }
        public double LoanAmount { get; set; }
        public double OptimalInterestRate { get; set; }
        public int OptimalMonthsPayBack { get; set; }

    }
}
