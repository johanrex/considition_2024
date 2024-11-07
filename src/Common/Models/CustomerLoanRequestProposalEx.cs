using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public record CustomerLoanRequestProposalEx : CustomerLoanRequestProposal
    {
        public double ScoreContribution { get; set; }
        public double LoanAmount { get; set; }
    }
}
