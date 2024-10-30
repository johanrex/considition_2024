using optimizer.Models.Pocos;
using optimizer.Models.Simulation;

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
            customer.name;
            //TODO grid search for optimal loan proposal

        }

    }
}
