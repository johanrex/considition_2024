using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Strategies
{
    internal class BruteForce
    {
        public List<CustomerPropositionDetails> Run(ServerUtils serverUtils, MapData map, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            Console.WriteLine("Starting brute force.");

            var details = new List<CustomerPropositionDetails>();

            for (int i = 0; i < map.customers.Length; i++)
            {
                var customer = map.customers[i];

                if (customer.name == "Glenn")
                {
                    Console.WriteLine("Skipping Glenn (the retard).");
                    continue;
                }

                Console.WriteLine($"Brute forcing {customer.name}. {i+1}/{map.customers.Length}.");

                var personality = PersonalityUtils.StringToEnum(customer.personality);
                var personalitySpec = personalities[personality];
                var acceptedMaxInterest = personalitySpec.AcceptedMaxInterest ?? 0.0;
                var acceptedMinInterest = personalitySpec.AcceptedMinInterest ?? 0.0;
                var maxMonthsToPayBackLoan = 30;

                var bestScore = 0.0;
                var optimalInterestRate = 0.0;
                var optimalMonthsToPayBackLoan = 0;

                for (double yearlyInterestRate = acceptedMinInterest; yearlyInterestRate <= acceptedMaxInterest; yearlyInterestRate += 0.001)
                {
                    for (int monthsToPayBackLoan = 1; monthsToPayBackLoan <= maxMonthsToPayBackLoan; monthsToPayBackLoan++)
                    {
                        var input = LoanUtils.CreateSingleCustomerGameInput(map.name, map.gameLengthInMonths, customer.name, yearlyInterestRate, monthsToPayBackLoan);
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
                    CustomerName = customer.name,
                    ScoreContribution = bestScore,
                    LoanAmount = customer.loan.amount,
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
