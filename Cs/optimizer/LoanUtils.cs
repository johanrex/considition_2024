using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static double CalculateProfitability(double loanAmount, double yearlyInterestRate, int monthsToPayBackLoan, double capital, double monthlyIncome, double monthlyExpenses, int nrOfKids, double mortgage, bool hasStudentLoans, Personality personality)
        {

            //return profitability;
            return 0.0;
        }

        public void CollectMonthlyPayment(int iterationIndex,
  List<Customer> customers,
  Map map)
        {
            foreach (Customer customer in customers)
            {
                CustomerAction customerAction = iteration.CustomerActions[customer.Name];
                customer.Payday();
                // ISSUE: reference to a compiler-generated field
                customer.PayBills(iterationIndex, this.\u003CconfigService\u003EP.Personalities);
                if (customer.CanPayLoan())
                    map.Budget += customer.PayLoan();
                else
                    customer.IncrementMark();
                if (customerAction.Type == CustomerActionType.Award)
                    map.Budget -= this.Award(customer, customerAction.Award);
            }
        }

    }
}
