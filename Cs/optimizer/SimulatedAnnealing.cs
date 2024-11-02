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
        private ServerUtils ServerUtils;

        private SimulatedAnnealing()
        {}

        public SimulatedAnnealing(ServerUtils serverUtils, string mapName, int gameLengthInMonths, string customerName, double startYearlyInterestRate, int startMonthsToPayBackLoan, double acceptedMinInterest, double acceptedMaxInterest, int maxMonthsToPayBackLoan)
        {
            // Set the properties
            MapName = mapName;
            GameLengthInMonths = gameLengthInMonths;
            CustomerName = customerName;
            AcceptedMinInterest = acceptedMinInterest;
            AcceptedMaxInterest = acceptedMaxInterest;
            MaxMonthsToPayBackLoan = maxMonthsToPayBackLoan;
            ServerUtils = serverUtils;

            // Set the optimization parameters
            YearlyInterestRate = startYearlyInterestRate;
            MonthsToPayBackLoan = startMonthsToPayBackLoan;
        }

        private double ScoreFunction(double yearlyInterestRate, int monthsToPayBackLoan)
        {
            var input = LoanUtils.CreateSingleCustomerGameInput(MapName, GameLengthInMonths, CustomerName, yearlyInterestRate, monthsToPayBackLoan);
            var gameResponse = ServerUtils.SubmitGameAsync(input).Result;
            var score = GameUtils.GetTotalScore(gameResponse);
            return score;
        }


        public (double, double, int) Run(
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
            double bestScore = ScoreFunction(x, y);
            double temperature = initialTemperature;

            for (int i = 0; i < maxIterations; i++)
            {
                // Perturb the variables
                double newX = x + (rand.NextDouble() - 0.5);
                int newY = y + (rand.Next(3) - 1); // Perturb y by -1, 0, or 1

                // Clamp the variables within their bounds
                newX = Math.Max(xMin, Math.Min(xMax, newX));
                newY = Math.Max(yMin, Math.Min(yMax, newY));

                double newScore = ScoreFunction(newX, newY);

                // Accept the new variables based on probability
                if (newScore > bestScore || Math.Exp((newScore - bestScore) / temperature) > rand.NextDouble())
                {
                    x = newX;
                    y = newY;
                    bestScore = newScore;
                    bestX = x;
                    bestY = y;
                }

                // Decrease the temperature
                temperature *= coolingRate;
            }

            return (bestScore, bestX, bestY);
        }
    }
}
