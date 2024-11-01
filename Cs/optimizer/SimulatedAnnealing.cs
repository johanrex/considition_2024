using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class SimulatedAnnealing
    {
        private string MapName;
        private int GameLengthInMonths;
        private string CustomerName;
        private double AcceptedMinInterest;
        private double AcceptedMaxInterest;
        private int MaxMonthsToPayBackLoan;
        private double YearlyInterestRate;
        private int MonthsToPayBackLoan;
        private GameUtils GameUtils;

        private SimulatedAnnealing()
        {}

        public SimulatedAnnealing(GameUtils gameUtils, string mapName, int gameLengthInMonths, string customerName, double startYearlyInterestRate, int startMonthsToPayBackLoan, double acceptedMinInterest, double acceptedMaxInterest, int maxMonthsToPayBackLoan)
        {
            // Set the properties
            MapName = mapName;
            GameLengthInMonths = gameLengthInMonths;
            CustomerName = customerName;
            AcceptedMinInterest = acceptedMinInterest;
            AcceptedMaxInterest = acceptedMaxInterest;
            MaxMonthsToPayBackLoan = maxMonthsToPayBackLoan;
            GameUtils = gameUtils;

            // Set the optimization parameters
            YearlyInterestRate = startYearlyInterestRate;
            MonthsToPayBackLoan = startMonthsToPayBackLoan;
        }

        private double CostFunction(double yearlyInterestRate, int monthsToPayBackLoan)
        {
            var input = LoanUtils.CreateSingleCustomerGameInput(MapName, GameLengthInMonths, CustomerName, yearlyInterestRate, monthsToPayBackLoan);
            var score = GameUtils.ScoreGame(input);
            return score;
        }

        public (double, int) Run(
            double x0,
            int y0,
            double initialTemperature,
            double coolingRate,
            int maxIterations,
            double xMin,
            double xMax,
            int yMin,
            int yMax)
        {
            Random rand = new Random();
            double x = x0;
            int y = y0;
            double bestX = x;
            int bestY = y;
            double bestCost = CostFunction(x, y);
            double temperature = initialTemperature;

            for (int i = 0; i < maxIterations; i++)
            {
                // Perturb the variables
                double newX = x + (rand.NextDouble() - 0.5);
                int newY = y + (rand.Next(3) - 1); // Perturb y by -1, 0, or 1

                // Clamp the variables within their bounds
                newX = Math.Max(xMin, Math.Min(xMax, newX));
                newY = Math.Max(yMin, Math.Min(yMax, newY));

                double newCost = CostFunction(newX, newY);

                // Accept the new variables based on probability
                if (newCost < bestCost || Math.Exp((bestCost - newCost) / temperature) > rand.NextDouble())
                {
                    x = newX;
                    y = newY;
                    bestCost = newCost;
                    bestX = x;
                    bestY = y;
                }

                // Decrease the temperature
                temperature *= coolingRate;
            }

            return (bestX, bestY);
        }
    }
}
