using Common.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IterationAwardsSimulatedAnnealing(
            Map map, 
            CustomerLoanRequestProposalEx proposalEx,
            ConfigService configService,
            Dictionary<string, Customer> mapCustomerLookup,
            Dictionary<Personality, PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards
            )
        {
            this.map = map;
            this.configService = configService;
            this.proposalEx = proposalEx;
            this.mapCustomerLookup = mapCustomerLookup;
            this.personalities = personalities;
            this.awards = awards;
            this.random = new Random();
        }

        public List<CustomerActionIteration> Run()
        {
            var initialState = GetInitialState(map.GameLengthInMonths, proposalEx.CustomerName);
            var currentState = initialState;
            var bestState = initialState;
            double currentScore = ScoreFunction();
            double bestScore = currentScore;

            double temperature = 1.0;
            double coolingRate = 0.003;

            int maxIterations = 1000; // Set your desired number of iterations here
            int iteration = 0;

            while (temperature > 0.1 && iteration < maxIterations)
            {
                var neighbor = GetNeighbor(currentState, proposalEx.CustomerName);
                double neighborScore = ScoreFunction();

                if (AcceptanceProbability(currentScore, neighborScore, temperature) > random.NextDouble())
                {
                    currentState = neighbor;
                    currentScore = neighborScore;
                }

                if (currentScore > bestScore)
                {
                    bestState = currentState;
                    bestScore = currentScore;
                }

                temperature *= 1 - coolingRate;
                iteration++;
            }

            return bestState;
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
                iterations.Add(new CustomerActionIteration());
            }
            return iterations;
        }

        private List<CustomerActionIteration> GetNeighbor(List<CustomerActionIteration> currentState, string customerName)
        {
            //This function randomly changes two things.

            List<CustomerActionIteration> neighbor = DeepCopy(currentState);

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

        List<CustomerActionIteration> DeepCopy(List<CustomerActionIteration> currentObj)
        {
            List<CustomerActionIteration> newObj = new();
            foreach (CustomerActionIteration iteration in currentObj)
            {
                Dictionary<string, CustomerAction> kvp = new();
                foreach (KeyValuePair<string, CustomerAction> pair in iteration.CustomerActions)
                {
                    kvp[pair.Key] = new CustomerAction
                    {
                        Type = pair.Value.Type,
                        Award = pair.Value.Award
                    };
                }
                CustomerActionIteration newIteration = new();
                newIteration.CustomerActions = kvp;
                newObj.Add(newIteration);
            }

            return newObj;
        }

        private double ScoreFunction()
        {
            var input = GameUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, proposalEx.CustomerName, proposalEx.YearlyInterestRate, proposalEx.MonthsToPayBackLoan);
            var scorer = new NativeScorer.NativeScorer(configService, personalities, awards);
            var gameResponse = scorer.RunGame(input, mapCustomerLookup);
            var score = GameUtils.GetTotalScore(gameResponse);
            return score;
        }


    }
}
