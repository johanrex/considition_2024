using Common.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Strategies
{
    internal class IndividualScoreBruteForce
    {
        public List<CustomerPropositionDetails> Run(ServerUtils serverUtils, Map map, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            Console.WriteLine("Starting brute force.");

            var details = new List<CustomerPropositionDetails>();

            for (int i = 0; i < map.Customers.Count; i++)
            {
                var customer = map.Customers[i];

                if (customer.Name == "Glenn")
                {
                    Console.WriteLine("Skipping Glenn (the retard).");
                    continue;
                }

                Console.WriteLine($"Brute forcing {customer.Name}. {i+1}/{map.Customers.Count}.");

                var personality = customer.Personality;
                var personalitySpec = personalities[personality];
                var acceptedMaxInterest = personalitySpec.AcceptedMaxInterest ?? 0.0;
                var acceptedMinInterest = personalitySpec.AcceptedMinInterest ?? 0.0;
                var maxMonthsToPayBackLoan = map.GameLengthInMonths*4;

                var bestScore = 0.0;
                var optimalInterestRate = 0.0;
                var optimalMonthsToPayBackLoan = 0;

                for (double yearlyInterestRate = acceptedMinInterest; yearlyInterestRate <= acceptedMaxInterest; yearlyInterestRate += 0.001)
                {
                    for (int monthsToPayBackLoan = 1; monthsToPayBackLoan <= maxMonthsToPayBackLoan; monthsToPayBackLoan++)
                    {
                        var input = GameUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, customer.Name, yearlyInterestRate, monthsToPayBackLoan);
                        var gameResponse = serverUtils.SubmitGameAsync(input).Result;
                        var score = GameUtils.GetTotalScore(gameResponse);

                        if (score > bestScore)
                        {
                            bestScore = score;
                            optimalInterestRate = yearlyInterestRate;
                            optimalMonthsToPayBackLoan = monthsToPayBackLoan;
                        }
                    }
                }

                var detail = new CustomerPropositionDetails
                {
                    CustomerName = customer.Name,
                    ScoreContribution = bestScore,
                    LoanAmount = customer.Loan.Amount,
                    OptimalInterestRate = optimalInterestRate,
                    OptimalMonthsPayBack = optimalMonthsToPayBackLoan
                };
                details.Add(detail);
            }
            Console.WriteLine("Brute force done.");

            return details;
        }
    }
}
