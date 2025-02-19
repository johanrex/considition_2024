﻿using Common.Models;
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
        private ScoreUtils scoreUtils;
        private Map map;
        private CustomerLoanRequestProposalEx proposalEx;
        private Random random;
        private double temperature;
        private double coolingRate;
        private int maxIterations;

        public IterationAwardsSimulatedAnnealing(
            ScoreUtils scoreUtils,
            Map map, 
            CustomerLoanRequestProposalEx proposalEx,
            double temperature,
            double coolingRate,
            int maxIterations
            )
        {

            this.scoreUtils = scoreUtils;
            this.map = map;
            this.proposalEx = proposalEx;
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

            double currentScore = ScoreFunction(currentState);
            double bestScore = currentScore;

            int iteration = 0;

            while (temperature > 0.1 && iteration < maxIterations)
            {
                var neighbor = GetNeighbor(currentState, proposalEx.CustomerName);
                double neighborScore = ScoreFunction(neighbor);

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

            CustomerLoanRequestProposalEx bestProposalEx = new CustomerLoanRequestProposalEx
            {
                CustomerName = proposalEx.CustomerName,
                YearlyInterestRate = proposalEx.YearlyInterestRate,
                MonthsToPayBackLoan = proposalEx.MonthsToPayBackLoan,
                TotalScore = bestScore,
                LoanAmount = proposalEx.LoanAmount,
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

            int awardTypeCount = Enum.GetValues(typeof(AwardType)).Length;
            AwardType randomType = (AwardType)random.Next(awardTypeCount);
            action.Award = randomType;
            action.Type = action.Award == AwardType.None ? CustomerActionType.Skip : CustomerActionType.Award;

            return neighbor;
        }

        private double ScoreFunction(List<CustomerActionIteration> currentState)
        {
            var input = GameUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, proposalEx.CustomerName, proposalEx.YearlyInterestRate, proposalEx.MonthsToPayBackLoan, currentState);
            var gameResult = this.scoreUtils.SubmitGame(input);
            var totalScore = gameResult.TotalScore;

            return totalScore;
        }
    }
}
