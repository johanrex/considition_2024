using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalHost.Models;
using optimizer.Models;
using optimizer.Models.Pocos;

namespace optimizer
{
    internal class LoanUtils
    {
        public static bool canBankLend(double budget, double loanAmount)
        {
            return budget >= loanAmount;
        }

        public static (double, CustomerLoanRequestProposal) FindOptimalLoanProposal(double initialBankCapital, optimizer.Models.Pocos.Customer customer, MapData map, Dictionary<string, PersonalitySpecification> personalities)
        {
            customer.name
            //TODO grid search for optimal loan proposal

        }

    }
}
