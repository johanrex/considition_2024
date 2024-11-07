using Common.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace optimizer.Strategies
{
    internal class IterationAwardsSimulatedAnnealing
    {
        private Map map;
        private ConfigService configService;
        private CustomerLoanRequestProposalEx proposalEx;
        private Dictionary<string, Customer> mapCustomerLookup;
        private Dictionary<Personality, PersonalitySpecification> personalities;
        private Dictionary<AwardType, AwardSpecification> awards;
        private Random random;
        private double temperature;
        private double coolingRate;
        private int maxIterations;

        public IterationAwardsSimulatedAnnealing(
            Map map, 
            CustomerLoanRequestProposalEx proposalEx,
            ConfigService configService,
            Dictionary<string, Customer> mapCustomerLookup,
            Dictionary<Personality, PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards,
            double temperature,
            double coolingRate,
            int maxIterations
            )
        {
            this.map = map;
            this.configService = configService;
            this.proposalEx = proposalEx;
            this.mapCustomerLookup = mapCustomerLookup;
            this.personalities = personalities;
            this.awards = awards;
            this.temperature = temperature;
            this.coolingRate = coolingRate;
            this.maxIterations = maxIterations;

            this.random = new Random();
        }

        public CustomerLoanRequestProposalEx Run()
        {
            var initialState = GetInitialState(map.GameLengthInMonths, proposalEx.CustomerName);
            var currentState = initialState;
            var bestState = initialState;

            (double currentScore, double currentTotalCost) = ScoreFunction(currentState);
            
            double bestScore = currentScore;
            double bestTotalCost = currentTotalCost;

            double temperature = 1.0;
            double coolingRate = 0.003;

            int maxIterations = 1000; 
            int iteration = 0;

            while (temperature > 0.1 && iteration < maxIterations)
            {
                var neighbor = GetNeighbor(currentState, proposalEx.CustomerName);
                (double neighborScore, double neighborTotalCost) = ScoreFunction(neighbor);

                if (AcceptanceProbability(currentScore, neighborScore, temperature) > random.NextDouble())
                {
                    currentState = neighbor;
                    currentScore = neighborScore;
                    currentTotalCost = neighborTotalCost;
                }

                if (currentScore > bestScore)
                {
                    bestState = currentState;
                    bestScore = currentScore;
                    bestTotalCost = currentTotalCost;
                }

                temperature *= 1 - coolingRate;
                iteration++;
            }

            CustomerLoanRequestProposalEx bestProposalEx = new CustomerLoanRequestProposalEx
            {
                CustomerName = proposalEx.CustomerName,
                YearlyInterestRate = proposalEx.YearlyInterestRate,
                MonthsToPayBackLoan = proposalEx.MonthsToPayBackLoan,
                TotalScore = bestScore,
                Cost = bestTotalCost,
                Iterations = bestState
            };

            return bestProposalEx;
        }

        private double AcceptanceProbability(double currentScore, double neighborScore, double temperature)
        {
            if (neighborScore > currentScore)
            {
                return 1.0;
            }
            return Math.Exp((neighborScore - currentScore) / temperature);
        }

        private List<CustomerActionIteration> GetInitialState(int gameLengthInMonths, string customerName)
        {
            List<CustomerActionIteration> iterations = new();
            for (int i = 0; i < gameLengthInMonths; i++)
            {
                Dictionary<string, CustomerAction> kvp = new();
                kvp[customerName] = new CustomerAction
                {
                    Type = CustomerActionType.Skip,
                    Award = AwardType.None
                };

                CustomerActionIteration iteration = new();
                iteration.CustomerActions = kvp;
                iterations.Add(iteration);
            }
            return iterations;
        }

        private List<CustomerActionIteration> GetNeighbor(List<CustomerActionIteration> currentState, string customerName)
        {
            //This function randomly changes two things.

            List<CustomerActionIteration> neighbor = ObjectUtils.DeepCopyWithJson(currentState);

            /////////////////////////////////////////////////////
            /// 1. Pick a random iteration (month) to change. ///
            /////////////////////////////////////////////////////
            int index = random.Next(neighbor.Count);
            CustomerActionIteration iteration = neighbor[index];
            Dictionary<string, CustomerAction> kvp = iteration.CustomerActions;

            /////////////////////////////////////////////////////
            /// 2. Change the action at random                ///
            /////////////////////////////////////////////////////
            CustomerAction action = kvp[customerName];
            if (action.Type == CustomerActionType.Award)
            {
                action.Type = CustomerActionType.Skip;
                action.Award = AwardType.None;
            }
            else
            {
                int awardTypeCount = Enum.GetValues(typeof(AwardType)).Length;
                AwardType randomType = (AwardType)random.Next(1, awardTypeCount); //we skip the "None"-award, the first one. 
                action.Award = randomType;
            }

            return neighbor;
        }

        private (double, double) ScoreFunction(List<CustomerActionIteration> currentState)
        {
            var input = GameUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, proposalEx.CustomerName, proposalEx.YearlyInterestRate, proposalEx.MonthsToPayBackLoan, currentState);
            var scorer = new NativeScorer.NativeScorer(configService, personalities, awards);
            var gameResponse = scorer.RunGame(input, mapCustomerLookup);
            var score = GameUtils.GetTotalScore(gameResponse);
            var totalCost = -scorer.expenses;

            return (score, totalCost);
        }


    }
}
