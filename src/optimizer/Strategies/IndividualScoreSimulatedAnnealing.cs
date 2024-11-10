using Common.Models;
using Common.Services;
using NativeScorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Strategies
{
    internal class IndividualScoreSimulatedAnnealing
    {
        private ServerUtils serverUtils;
        private Map map;
        private string customerName;
        private double acceptedMinInterest;
        private double acceptedMaxInterest;
        private int maxMonthsToPayBackLoan;
        private double YearlyInterestRate;
        private int MonthsToPayBackLoan;

        private IndividualScoreSimulatedAnnealing()
        { }

        public IndividualScoreSimulatedAnnealing(
            ServerUtils serverUtils,
            Map map, 
            string customerName, 
            double startYearlyInterestRate, 
            int startMonthsToPayBackLoan, 
            double acceptedMinInterest, 
            double acceptedMaxInterest, 
            int maxMonthsToPayBackLoan)
        {
            // Set the properties
            this.serverUtils = serverUtils;
            this.map = map;
            this.customerName = customerName;
            this.acceptedMinInterest = acceptedMinInterest;
            this.acceptedMaxInterest = acceptedMaxInterest;
            this.maxMonthsToPayBackLoan = maxMonthsToPayBackLoan;

            // Set the optimization parameters
            YearlyInterestRate = startYearlyInterestRate;
            MonthsToPayBackLoan = startMonthsToPayBackLoan;
        }

        private double ScoreFunction(double yearlyInterestRate, int monthsToPayBackLoan)
        {
            //TODO this function may not be needed when I have a better. 
            var input = GameUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, customerName, yearlyInterestRate, monthsToPayBackLoan);
            var gameResponse = serverUtils.SubmitGameAsync(input).Result;
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
