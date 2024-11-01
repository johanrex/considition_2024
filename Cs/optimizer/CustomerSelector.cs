using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimizer.Models.Pocos;

namespace optimizer
{
    internal class CustomerSelector
    {
        public static List<CustomerPropositionDetails> Select(MapData map, List<CustomerPropositionDetails> customerDetails)
        {
            //TODO verify that customer names are unique when reading the map.

            //TODO how do we select the most profitable customers within our budget?
            //Is this a knapsack problem? Are there other approaches?

            /*
            Customer name: Gordon, bestScore: 32024,999999999993, optimalInterestRate: 0,02, optimalMonthsToPayBackLoan: 18, loanAmount: 800000,00.
            Customer name: Glenn, bestScore: -11100, optimalInterestRate: 49996,461425335634, optimalMonthsToPayBackLoan: 10, loanAmount: 300000,00.
            Customer name: Kim, bestScore: 500,0000000000001, optimalInterestRate: 0,1, optimalMonthsToPayBackLoan: 1, loanAmount: 2000,00.
            Customer name: Emil, bestScore: 2074,999999999999, optimalInterestRate: 0,05, optimalMonthsToPayBackLoan: 35, loanAmount: 20000,00.
            Customer name: Ada, bestScore: 249999,9999999999, optimalInterestRate: 0,5, optimalMonthsToPayBackLoan: 28, loanAmount: 250000,00.
            */

            //The bank's budget
            var budget = map.budget;



            return null;

        }
    }
}
