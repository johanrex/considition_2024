using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class SimulatedAnnealingFacade
    {
        public static void Run(GameUtils gameUtils, MapData map, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            for (int i = 0; i < map.customers.Length; i++)
            {
                //Let's test simulated annealing
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
                    gameUtils,
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

                Console.WriteLine($"Customer name: {customerName}, bestScore: {bestScore}, optimalInterestRate: {optimalInterestRate}, optimalMonthsToPayBackLoan: {optimalMonthsToPayBackLoan}.");


            }


        }
    }
}
