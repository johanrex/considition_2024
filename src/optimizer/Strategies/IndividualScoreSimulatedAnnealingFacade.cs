
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;

namespace optimizer.Strategies
{
    internal class IndividualScoreSimulatedAnnealingFacade
    {
        public static List<CustomerPropositionDetails> Run(
            ConfigService configService,
            Map map, Dictionary<Personality, 
            PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards)
        {
            Console.WriteLine("Starting simulated annealing.");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            var mapCustomerLookup = map.Customers.ToDictionary(c => c.Name);

            var details = new ConcurrentBag<CustomerPropositionDetails>();

            var startMonthsToPayBackLoan = map.GameLengthInMonths / 2;
            var maxMonthsToPayBackLoan = map.GameLengthInMonths * 4;
            var initialTemperature = 1000.0;
            var coolingRate = 0.95;
            var maxIterations = 1000;
            var retries = 3;

            Parallel.For(0, map.Customers.Count, i =>
            {
                // Let's test simulated annealing
                var customer = map.Customers[i];
                var customerName = customer.Name;
                var personality = customer.Personality;
                var personalitySpec = personalities[personality];
                var acceptedMaxInterest = personalitySpec.AcceptedMaxInterest ?? 0.0;
                var acceptedMinInterest = personalitySpec.AcceptedMinInterest ?? 0.0;
                var startYearlyInterestRate = (acceptedMaxInterest - acceptedMinInterest) / 2 + acceptedMinInterest;

                double bestScore=double.MinValue;
                double optimalInterestRate = double.MinValue;
                int optimalMonthsToPayBackLoan = int.MinValue;

                for (int _ = 0; _ < retries; _++)
                {
                    IndividualScoreSimulatedAnnealing anneal = new IndividualScoreSimulatedAnnealing(
                        configService,
                        map,
                        mapCustomerLookup,
                        personalities,
                        awards,
                        customerName,
                        startYearlyInterestRate,
                        startMonthsToPayBackLoan,
                        acceptedMinInterest,
                        acceptedMaxInterest,
                        maxMonthsToPayBackLoan);

                    (var runScore, var runInterestRate, var runMonthsToPayBackLoan) = anneal.Run(
                        startYearlyInterestRate,
                        startMonthsToPayBackLoan,
                        initialTemperature,
                        coolingRate,
                        maxIterations,
                        acceptedMinInterest,
                        acceptedMaxInterest,
                        0,
                        maxMonthsToPayBackLoan);

                    if (runScore > bestScore)
                    {
                        bestScore = runScore;
                        optimalInterestRate = runInterestRate;
                        optimalMonthsToPayBackLoan = runMonthsToPayBackLoan;
                    }
                }


                var detail = new CustomerPropositionDetails
                {
                    CustomerName = customerName,
                    ScoreContribution = bestScore,
                    LoanAmount = customer.Loan.Amount,
                    OptimalInterestRate = optimalInterestRate,
                    OptimalMonthsPayBack = optimalMonthsToPayBackLoan
                };
                details.Add(detail);

                //Log progress
                string msg = $"({details.Count}/{map.Customers.Count})";
                Console.WriteLine(msg);
            });

            // Stop the stopwatch
            stopwatch.Stop();

            // Calculate total time and customers per second
            double totalTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
            double customersPerSecond = map.Customers.Count / totalTimeInSeconds;


            Console.WriteLine($"maxMonthsToPayBackLoan: {maxMonthsToPayBackLoan}");
            Console.WriteLine($"initialTemperature: {initialTemperature}");
            Console.WriteLine($"coolingRate: {coolingRate}");
            Console.WriteLine($"maxIterations: {maxIterations}");
            Console.WriteLine($"Simulated annealing total time taken: {totalTimeInSeconds} seconds");
            Console.WriteLine($"Customers processed per second: {customersPerSecond}");



            return details.ToList();
        }
    }
}