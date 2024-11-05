﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Common.Services;
using Common.Models;


namespace NativeScorer
{
    public class NativeScorer
    {
        private readonly Dictionary<Personality, PersonalitySpecification> personalities;
        private double budget;
        private readonly Dictionary<AwardType, AwardSpecification> awards;

        private ConfigService configService;

        private NativeScorer()
        { }

        public NativeScorer(
            ConfigService configService,
            Dictionary<Personality, PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards
            )
        {
            this.configService = configService;
            this.personalities = personalities;
            this.awards = awards;
        }
        public static T DeepCopy<T>(T record)
        {
            var json = JsonSerializer.Serialize(record);
            return JsonSerializer.Deserialize<T>(json);
        }

        public GameResponse RunGame(GameInput gameInput, Dictionary<string, Customer> mapCustomerLookup)
        {
            var map = configService.GetMap(gameInput.MapName);
            List<Customer> customerList = RequestCustomers(gameInput, map, mapCustomerLookup);

            budget = map.Budget;

            double num = customerList.Sum(c => c.Loan.Amount);
            budget -= num;

            string errorMessage = HandleIterations(gameInput.Iterations, customerList, map);
            GameResult gameResult = CalculateScoreAndSaveGame(gameInput, customerList);
            return new GameResponse()
            {
                GameId = Guid.Empty,
                Score = gameResult
            };
        }

        public List<Customer> RequestCustomers(GameInput gameInput, Map map, Dictionary<string, Customer> mapCustomerLookup)
        {
            //ooof jag har bara en kund i min gameInput i normalfallet. Dvs jag behöver definitivt inte kopiera alla customers från map. 
            //Behöver bara ett snabbt sätt att hitta kunden i map.

            List<Customer> acceptedCustomers = new List<Customer>();

            //TODO silly proposal1, remove
            foreach (var proposal1 in gameInput.Proposals)
            {
                CustomerLoanRequestProposal proposal = proposal1;

                var customer = DeepCopy(mapCustomerLookup[proposal.CustomerName]);

                //if (customer.Propose(proposal.YearlyInterestRate, proposal.MonthsToPayBackLoan, this.personalities))
                if ((object)customer != null && customer.Propose(proposal.YearlyInterestRate, proposal.MonthsToPayBackLoan, personalities))
                    acceptedCustomers.Add(customer);
            }
            return acceptedCustomers;
        }

        private string HandleIterations(
          List<CustomerActionIteration> iterations,
          List<Customer> customers,
          Map map)
        {
            for (int index = 0; index < iterations.Count; ++index)
            {
                string str = CollectPayments(iterations[index], index, customers, map);
                if (str != null)
                    return str;
            }
            return null;
        }


        public string CollectPayments(
            CustomerActionIteration iteration,
            int month,
            List<Customer> customers,
            Map map)
        {
            string name = map.Name;

            Dictionary<Personality, PersonalitySpecification> personalitySpecifications = configService.GetPersonalitySpecifications(name);
            Dictionary<AwardType, AwardSpecification> awardSpecifications = configService.GetAwardSpecifications(name);

            foreach (Customer customer in customers)
            {
                if (map.Budget <= 0.0)
                    return "Your bank went bankrupt";
                if (!customer.IsBankrupt)
                {
                    CustomerAction customerAction = iteration.CustomerActions[customer.Name];
                    customer.Payday();
                    customer.PayBills(month, personalitySpecifications);
                    if (customer.CanPayLoan())
                        map.Budget += customer.PayLoan();
                    else
                        customer.IncrementMark();
                    if (customerAction.Type == CustomerActionType.Award)
                    {
                        double num = this.Award(customer, customerAction.Award, awardSpecifications, personalitySpecifications);
                        customer.Profit -= num;
                        map.Budget -= num;
                    }
                    else if (customer.AwardsInRow > 0)
                        --customer.AwardsInRow;

                }
            }
            return null;
        }


        private double Award(
          Customer customer,
          AwardType award,
          Dictionary<AwardType, AwardSpecification> awardSpecs,
          Dictionary<Personality, PersonalitySpecification> personalitySpecs)
        {
            double num = Math.Round((100.0 - customer.AwardsInRow * 20.0) / 100.0, 1);
            if (customer.AwardsInRow < 5)
                ++customer.AwardsInRow;
            switch (award)
            {
                case AwardType.IkeaCheck:
                    AwardSpecification awardSpec1 = awardSpecs[AwardType.IkeaCheck];
                    customer.Happiness += awardSpec1.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * num;
                    return awardSpec1.Cost;
                case AwardType.IkeaFoodCoupon:
                    AwardSpecification awardSpec2 = awardSpecs[AwardType.IkeaFoodCoupon];
                    customer.Happiness += awardSpec2.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * num;
                    return awardSpec2.Cost;
                case AwardType.IkeaDeliveryCheck:
                    AwardSpecification awardSpec3 = awardSpecs[AwardType.IkeaDeliveryCheck];
                    customer.Happiness += awardSpec3.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * num;
                    return awardSpec3.Cost;
                case AwardType.NoInterestRate:
                    AwardSpecification awardSpec4 = awardSpecs[AwardType.NoInterestRate];
                    customer.Happiness += awardSpec4.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * num;
                    return customer.Loan.GetInterestPayment() + awardSpec4.Cost;
                case AwardType.GiftCard:
                    AwardSpecification awardSpec5 = awardSpecs[AwardType.GiftCard];
                    customer.Happiness += awardSpec5.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * num;
                    return awardSpec5.Cost;
                case AwardType.HalfInterestRate:
                    AwardSpecification awardSpec6 = awardSpecs[AwardType.HalfInterestRate];
                    customer.Happiness += awardSpec6.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * num;
                    return customer.Loan.GetInterestPayment() / 2.0 + awardSpec6.Cost;
                default:
                    throw new ArgumentOutOfRangeException(nameof(award), award, null);
            }
        }

        public GameResult CalculateScoreAndSaveGame(
            GameInput gameInput,
            List<Customer> customers)
        {
            GameResult gameResult = new GameResult()
            {
                TotalProfit = (long)customers.Sum<Customer>((Func<Customer, double>)(x => x.Profit)),
                HappinessScore = (long)customers.Sum<Customer>((Func<Customer, double>)(x => x.Happiness)),
                EnvironmentalImpact = (long)customers.Sum<Customer>((Func<Customer, double>)(x => x.Loan.EnvironmentalImpact)),
                MapName = gameInput.MapName
            };
            return gameResult;
        }
    }
}