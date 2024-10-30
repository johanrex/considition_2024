using Microsoft.VisualBasic;
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

        public static (double, CustomerLoanRequestProposal?) FindOptimalLoanProposal(MapData map, Models.Pocos.Customer mCust, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            // can we afford this loan?
            if (mCust.loan.amount > map.budget)
            {
                return (0.0, null);
            }

            //TODO grid search for optimal loan proposal
            var yearlyInterestRate = 0.01;
            var monthsToPayBack = 12;


            Models.Simulation.Customer sCust = new Models.Simulation.Customer
            {
                Name = mCust.name,
                Capital = (double)mCust.capital,
                Income = (double)mCust.income,
                MonthlyExpenses = (double)mCust.monthlyExpenses,
                NumberOfKids = mCust.numberOfKids,
                Mortgage = (double)mCust.homeMortgage,
                HasStudentLoans = mCust.hasStudentLoan,
                Personality = (Personality)Enum.Parse(typeof(Personality), mCust.personality),
                Loan = new Models.Simulation.Loan
                {
                    Amount = (double)mCust.loan.amount,
                    Product = mCust.loan.product,
                    EnvironmentalImpact = (double)mCust.loan.environmentalImpact
                }

            };

            double retTotalPayment = 0.0;
            CustomerLoanRequestProposal retProposal = null;

            if (sCust.Propose(yearlyInterestRate, monthsToPayBack, personalities))
            {
                //code copied from IterationService.CollectPayments

                for (int i = 0; i < monthsToPayBack; i++) 
                {
                    sCust.Payday();

                    sCust.PayBills(i, personalities);
                    if (sCust.CanPayLoan())
                        retTotalPayment += sCust.PayLoan();
                    else
                        sCust.IncrementMark();

                    //CustomerAction customerAction = iteration.CustomerActions[sCust.Name];
                    //if (customerAction.Type == CustomerActionType.Award)
                    //    map.Budget -= this.Award(customer, customerAction.Award);
                    int j = 0;
                }

                retProposal = new CustomerLoanRequestProposal
                {
                    CustomerName = mCust.name,
                    YearlyInterestRate = yearlyInterestRate,
                    MonthsToPayBackLoan = monthsToPayBack
                };
            }

            return (retTotalPayment, retProposal);
        }
    }
}
