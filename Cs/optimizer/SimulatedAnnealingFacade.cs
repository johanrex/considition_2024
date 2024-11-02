using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class SimulatedAnnealingFacade
    {
        public static List<CustomerPropositionDetails> Run(ServerUtils serverUtils, MapData map, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            var details = new ConcurrentBag<CustomerPropositionDetails>();

            // TODO since we're IO bound by network traffic it's possible to use an async/await with Task instead of Parallel.For. Might be more efficient.
            // From copilot: When dealing with I/O-bound operations, using parallelism with Parallel.For might not be the most efficient approach. Instead, you should consider using asynchronous programming to handle I/O-bound tasks more effectively.
            Parallel.For(0, map.customers.Length, i =>
            {
                // Let's test simulated annealing
                var customer = map.customers[i];
                var customerName = customer.name;
                var personality = PersonalityUtils.StringToEnum(customer.personality);
                var personalitySpec = personalities[personality];
                var acceptedMaxInterest = personalitySpec.AcceptedMaxInterest ?? 0.0;
                var acceptedMinInterest = personalitySpec.AcceptedMinInterest ?? 0.0;
                var startYearlyInterestRate = (acceptedMaxInterest - acceptedMinInterest) / 2 + acceptedMinInterest;
                var startMonthsToPayBackLoan = map.gameLengthInMonths / 2;
                var maxMonthsToPayBackLoan = 50 * 12;
                var initialTemperature = 1000.0;
                var coolingRate = 0.95;
                var maxIterations = 1000;

                SimulatedAnnealing anneal = new SimulatedAnnealing(
                    serverUtils,
                    map.name,
                    map.gameLengthInMonths,
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
                    LoanAmount = customer.loan.amount,
                    OptimalInterestRate = optimalInterestRate,
                    OptimalMonthsPayBack = optimalMonthsToPayBackLoan
                };
                details.Add(detail);

                Console.WriteLine(detail.ToString());
            });

            // Stop the stopwatch
            stopwatch.Stop();

            // Calculate total time and customers per second
            double totalTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
            double customersPerSecond = map.customers.Length / totalTimeInSeconds;

            Console.WriteLine($"Simulated annealing total time taken: {totalTimeInSeconds} seconds");
            Console.WriteLine($"Customers processed per second: {customersPerSecond}");

            return details.ToList();
        }
    }
}