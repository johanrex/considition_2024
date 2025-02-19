﻿
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;

namespace optimizer.Strategies
{
    internal class IndividualScoreSimulatedAnnealingFacade
    {
        public static List<CustomerLoanRequestProposalEx> Run(
            ScoreUtils scoreUtils,
            Map map,
            Dictionary<Personality, PersonalitySpecification> personalities,
            int maxDegreeOfParallelism,
            Dictionary<string, Customer> nameCustomers)
        {
            Console.WriteLine("Single customer. Interest rate and length. Simulated annealing approach. ");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            var details = new ConcurrentBag<CustomerLoanRequestProposalEx>();
            
            var startMonthsToPayBackLoan = map.GameLengthInMonths / 2;
            var maxMonthsToPayBackLoan = map.GameLengthInMonths * 4; //TODO is this actually a good value?
            //var maxMonthsToPayBackLoan = int.MaxValue;
            //var maxMonthsToPayBackLoan = new Random().Next(1000, 3001);

            var initialTemperature = 1000.0;
            var coolingRate = 0.95;
            var maxIterations = 2000;
            var retries = 3;

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = maxDegreeOfParallelism;

            Parallel.ForEach(nameCustomers, options, kvp =>
            {
                var customer = kvp.Value;
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
                        scoreUtils,
                        map,
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


                var detail = new CustomerLoanRequestProposalEx
                {
                    CustomerName = customerName,
                    TotalScore = bestScore,
                    LoanAmount = customer.Loan.Amount,
                    YearlyInterestRate = optimalInterestRate,
                    MonthsToPayBackLoan = optimalMonthsToPayBackLoan
                };
                details.Add(detail);

                //Log progress
                string msg = $"Single customer loan length and interest rate ({details.Count}/{map.Customers.Count})";
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