using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Strategies
{
    internal class SimulatedAnnealingFacade
    {
        public static List<CustomerPropositionDetails> Run(
            Map map, Dictionary<Personality, 
            PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards)
        {
            Console.WriteLine("Starting simulated annealing.");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            var details = new ConcurrentBag<CustomerPropositionDetails>();

            var startMonthsToPayBackLoan = map.GameLengthInMonths / 2;
            var maxMonthsToPayBackLoan = 50 * 12;
            var initialTemperature = 1000.0;
            var coolingRate = 0.95;
            var maxIterations = 1000;

            // TODO since we're IO bound by network traffic it's possible to use an async/await with Task instead of Parallel.For. Might be more efficient.
            // From copilot: When dealing with I/O-bound operations, using parallelism with Parallel.For might not be the most efficient approach. Instead, you should consider using asynchronous programming to handle I/O-bound tasks more effectively.
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

                SimulatedAnnealing anneal = new SimulatedAnnealing(
                    map,
                    personalities,
                    awards,
                    customerName,
                    startYearlyInterestRate,
                    startMonthsToPayBackLoan,
                    acceptedMinInterest,
                    acceptedMaxInterest,
                    maxMonthsToPayBackLoan);

                (var bestScore, var optimalInterestRate, var optimalMonthsToPayBackLoan) = anneal.Run(
                    startYearlyInterestRate,
                    startMonthsToPayBackLoan,
                    initialTemperature,
                    coolingRate,
                    maxIterations,
                    acceptedMinInterest,
                    acceptedMaxInterest,
                    0,
                    maxMonthsToPayBackLoan);

                var detail = new CustomerPropositionDetails
                {
                    CustomerName = customerName,
                    ScoreContribution = bestScore,
                    LoanAmount = customer.Loan.Amount,
                    OptimalInterestRate = optimalInterestRate,
                    OptimalMonthsPayBack = optimalMonthsToPayBackLoan
                };
                details.Add(detail);

                //Console.WriteLine(detail.ToString());
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