using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimizer.Models;

namespace optimizer
{
    internal class LoanUtils
    {

        public static bool canBankLend(decimal budget, decimal loanAmount)
        {
            return budget >= loanAmount;
        }

        public static decimal CalculateProfitability(decimal loanAmount, decimal yearlyInterestRate, int monthsToPayBackLoan, decimal capital, decimal monthlyIncome, decimal monthlyExpenses, int nrOfKids, decimal mortgage, bool hasStudentLoans, Personality personality)
        {



            //return profitability;
            return 0.0M;
        }
    }
}
