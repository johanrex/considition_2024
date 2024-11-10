using Microsoft.VisualBasic;
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
        private ConfigService configService;

        private NativeScorer()
        { }

        public NativeScorer(
            ConfigService configService
            )
        {
            this.configService = configService;
        }

        public GameResponse RunGame(GameInput gameInput)
        {
            var map = configService.GetMap(gameInput.MapName);

            List<Customer> customerList = RequestCustomers(gameInput, map);

            double num = customerList.Sum(c => c.Loan.Amount);
            map.Budget -= num;

            string errorMessage = HandleIterations(gameInput.Iterations, customerList, map);
            GameResult gameResult = CalculateScoreAndSaveGame(gameInput, customerList);
            return new GameResponse()
            {
                GameId = Guid.Empty,
                Score = gameResult
            };
        }

        public List<Customer> RequestCustomers(GameInput gameInput, Map map)
        {
            List<Customer> customerList = new List<Customer>();
            Dictionary<Personality, PersonalitySpecification> personalitySpecifications = this.configService.GetPersonalitySpecifications(map.Name);
            foreach (var proposal1 in gameInput.Proposals)
            {
                CustomerLoanRequestProposal proposal = proposal1;
                //TODO performance. Could make a dictionary in a MapEx class instead. 
                Customer customer = map.Customers.FirstOrDefault<Customer>((Func<Customer, bool>)(c => c.Name.Equals(proposal.CustomerName)));

                if ((object)customer != null && customer.Propose(proposal.YearlyInterestRate, proposal.MonthsToPayBackLoan, personalitySpecifications, map.GameLengthInMonths))
                    customerList.Add(customer);
            }
            return customerList;
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
                        customer.MonthsWithoutAwardsInRow = 0;
                        customer.AwardsReceived.Add(customerAction.Award);
                        double num = this.Award(customer, customerAction.Award, awardSpecifications, personalitySpecifications);
                        customer.Profit -= num;
                        map.Budget -= num;
                    }
                    else
                    {
                        ++customer.MonthsWithoutAwardsInRow;
                        if (customer.MonthsWithoutAwardsInRow > 3)
                            customer.Happiness -= (double)(500 * customer.MonthsWithoutAwardsInRow);
                        if (customer.AwardsInRow > 0)
                            --customer.AwardsInRow;
                    }
                }
            }
            return (string)null;
        }


        private double Award(
          Customer customer,
          AwardType award,
          Dictionary<AwardType, AwardSpecification> awardSpecs,
          Dictionary<Personality, PersonalitySpecification> personalitySpecs)
        {
            double num = Math.Round((100.0 - (double)customer.AwardsInRow * 20.0) / 100.0, 1);
            if (customer.AwardsInRow < 5)
                ++customer.AwardsInRow;
            if (customer.AwardsReceived.Count >= 3)
            {
                List<AwardType> awardsReceived1 = customer.AwardsReceived;
                AwardType awardType1 = awardsReceived1[awardsReceived1.Count - 1];
                List<AwardType> awardsReceived2 = customer.AwardsReceived;
                AwardType awardType2 = awardsReceived2[awardsReceived2.Count - 2];
                List<AwardType> awardsReceived3 = customer.AwardsReceived;
                AwardType awardType3 = awardsReceived3[awardsReceived3.Count - 3];
                if (awardType1 == awardType2 && awardType2 == awardType3)
                    num = -1.0;
            }
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
                    throw new ArgumentOutOfRangeException(nameof(award), (object)award, (string)null);
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