using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Common.Services;
using Common.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Runtime.CompilerServices;


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

            //string str = ValidateGameInput(gameInput, map);
            //if (str != null)
            //    return new GameResponse() { Message = str };

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

        private static string? ValidateGameInput(GameInput gameInput, Map map)
        {
            if (gameInput.Proposals.Count == 0)
                return "You must choose at least one customer to play!";
            if (gameInput.Iterations.Count > map.GameLengthInMonths)
                return "You can not exceed amount of months in 'iterations' then described in map config";
            if (map.GameLengthInMonths != gameInput.Iterations.Count)
                return "You must provide customer actions for each month of the designated game lenght!";
            if (gameInput.Proposals.Any<CustomerLoanRequestProposal>((Func<CustomerLoanRequestProposal, bool>)(proposal => proposal.MonthsToPayBackLoan < 0)))
                return "Customers need at least one month to pay back loan";
            IEnumerable<string> mapCustomerNames = map.Customers.Select<Customer, string>((Func<Customer, string>)(c => c.Name));
            if (gameInput.Proposals.Any<CustomerLoanRequestProposal>((Func<CustomerLoanRequestProposal, bool>)(proposal => !mapCustomerNames.Contains<string>(proposal.CustomerName))))
                return "All requested customers must exist on the chosen map!";
            if (gameInput.Proposals.Sum<CustomerLoanRequestProposal>((Func<CustomerLoanRequestProposal, double>)(x =>
            {
                Customer customer = map.Customers.FirstOrDefault<Customer>((Func<Customer, bool>)(y => y.Name == x.CustomerName));
                return (object)customer == null ? 0.0 : customer.Loan.Amount;
            })) > map.Budget)
            {
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
                interpolatedStringHandler.AppendLiteral("Tried starting game without sufficient funds, budget: ");
                interpolatedStringHandler.AppendFormatted<double>(map.Budget);
                return interpolatedStringHandler.ToStringAndClear();
            }
            HashSet<string> customerNames = new HashSet<string>();
            foreach (CustomerLoanRequestProposal proposal in gameInput.Proposals)
            {
                if (!customerNames.Add(proposal.CustomerName))
                    return "Customer '" + proposal.CustomerName + "' is already on the chosen map!";
            }
            for (int index = 0; index < gameInput.Iterations.Count; ++index)
            {
                if (!SequenceEqualsAnyOrder((ICollection<string>)gameInput.Iterations[index].CustomerActions.Keys.ToList<string>(), (ICollection<string>)customerNames))
                {
                    DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 1);
                    interpolatedStringHandler.AppendLiteral("Customer names in iteration ");
                    interpolatedStringHandler.AppendFormatted<int>(index);
                    interpolatedStringHandler.AppendLiteral(" doesn't match customers in proposal list");
                    return interpolatedStringHandler.ToStringAndClear();
                }
            }
            return (string)null;
        }

        private static bool SequenceEqualsAnyOrder(
          ICollection<string> iterationCustomerNames,
          ICollection<string> customerNames)
        {
            return iterationCustomerNames.Count == customerNames.Count && !iterationCustomerNames.Except<string>((IEnumerable<string>)customerNames).Any<string>();
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


        public string? CollectPayments(CustomerActionIteration iteration, int month, List<Customer> customers, Map map)
        {
            string mapName = map.Name;
            Dictionary<Personality, PersonalitySpecification> personalitySpecs = configService.GetPersonalitySpecifications(mapName);
            Dictionary<AwardType, AwardSpecification> awardSpecs = configService.GetAwardSpecifications(mapName);
            foreach (Customer customer in customers)
            {
                if (map.Budget <= 0.0)
                {
                    return "Your bank went bankrupt";
                }
                if (customer.IsBankrupt)
                {
                    continue;
                }
                CustomerAction customerAction = iteration.CustomerActions[customer.Name];
                customer.Payday();
                customer.PayBills(month, personalitySpecs);
                if (customer.CanPayLoan())
                {
                    map.Budget += customer.PayLoan();
                }
                else
                {
                    customer.IncrementMark();
                }
                if (customerAction.Type == CustomerActionType.Award)
                {
                    customer.MonthsWithoutAwardsInRow = 0;
                    customer.AwardsReceived.Add(customerAction.Award);
                    double awardCost = Award(customer, customerAction.Award, awardSpecs, personalitySpecs);
                    customer.Profit -= awardCost;
                    map.Budget -= awardCost;
                    continue;
                }
                customer.MonthsWithoutAwardsInRow++;
                if (customer.MonthsWithoutAwardsInRow > 3)
                {
                    customer.Happiness -= 500 * customer.MonthsWithoutAwardsInRow;
                }
                if (customer.AwardsInRow > 0)
                {
                    customer.AwardsInRow--;
                }
            }
            return null;
        }


        private double Award(Customer customer, AwardType award, Dictionary<AwardType, AwardSpecification> awardSpecs, Dictionary<Personality, PersonalitySpecification> personalitySpecs)
        {
            double happinessMultiplier = 100.0 - (double)customer.AwardsInRow * 20.0;
            happinessMultiplier /= 100.0;
            happinessMultiplier = Math.Round(happinessMultiplier, 1);
            if (customer.AwardsInRow < 5)
            {
                customer.AwardsInRow++;
            }
            if (customer.AwardsReceived.Count >= 3)
            {
                List<AwardType> awardsReceived = customer.AwardsReceived;
                AwardType last = awardsReceived[awardsReceived.Count - 1];
                List<AwardType> awardsReceived2 = customer.AwardsReceived;
                AwardType secondLast = awardsReceived2[awardsReceived2.Count - 2];
                List<AwardType> awardsReceived3 = customer.AwardsReceived;
                AwardType thirdLast = awardsReceived3[awardsReceived3.Count - 3];
                if (last == secondLast && secondLast == thirdLast)
                {
                    happinessMultiplier = -1.0;
                }
            }
            switch (award)
            {
                case AwardType.IkeaCheck:
                    {
                        AwardSpecification ikeaCheck = awardSpecs[AwardType.IkeaCheck];
                        customer.Happiness += ikeaCheck.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * happinessMultiplier;
                        return ikeaCheck.Cost;
                    }
                case AwardType.IkeaFoodCoupon:
                    {
                        AwardSpecification ikeaFoodCoupon = awardSpecs[AwardType.IkeaFoodCoupon];
                        customer.Happiness += ikeaFoodCoupon.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * happinessMultiplier;
                        return ikeaFoodCoupon.Cost;
                    }
                case AwardType.IkeaDeliveryCheck:
                    {
                        AwardSpecification ikeaDeliveryCheck = awardSpecs[AwardType.IkeaDeliveryCheck];
                        customer.Happiness += ikeaDeliveryCheck.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * happinessMultiplier;
                        return ikeaDeliveryCheck.Cost;
                    }
                case AwardType.GiftCard:
                    {
                        AwardSpecification giftCard = awardSpecs[AwardType.GiftCard];
                        customer.Happiness += giftCard.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * happinessMultiplier;
                        return giftCard.Cost;
                    }
                case AwardType.NoInterestRate:
                    {
                        AwardSpecification noInterest = awardSpecs[AwardType.NoInterestRate];
                        customer.Happiness += noInterest.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * happinessMultiplier;
                        return customer.Loan.GetInterestPayment() + noInterest.Cost;
                    }
                case AwardType.HalfInterestRate:
                    {
                        AwardSpecification halfInterest = awardSpecs[AwardType.HalfInterestRate];
                        customer.Happiness += halfInterest.BaseHappiness * personalitySpecs[customer.Personality].HappinessMultiplier * happinessMultiplier;
                        return customer.Loan.GetInterestPayment() / 2.0 + halfInterest.Cost;
                    }
                default:
                    throw new ArgumentOutOfRangeException("award", award, null);
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