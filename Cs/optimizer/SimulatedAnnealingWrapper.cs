using optimizer.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class SimulatedAnnealingWrapper
    {
        public string MapName { get; private set; }
        public int GameLengthInMonths { get; private set; }
        public string CustomerName { get; private set; }
        public double YearlyInterestRate { get; private set; }
        public int MonthsToPayBackLoan { get; private set; }

        // Initialize method to set the properties
        public void Initialize(string mapName, int gameLengthInMonths, string customerName, double yearlyInterestRate, int monthsToPayBackLoan, double acceptedMinInterest, double acceptedMaxInterest, int maxMonthsToPayBackLoan)
        {
        }
    }
}
