using Microsoft.VisualBasic;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace optimizer
{
    internal class NativeScorer
    {
        private readonly Dictionary<Personality, PersonalitySpecification> personalities;
        private double budget;
        private readonly Dictionary<AwardType, AwardSpecification> awards;

        private NativeScorer()
        {}

        public NativeScorer(
            Dictionary<Personality, PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards
            )
        {
            this.personalities = personalities;
            this.awards = awards;
        }
        public static T DeepCopy<T>(T record)
        {
            var json = JsonSerializer.Serialize(record);
            return JsonSerializer.Deserialize<T>(json);
        }

        public GameResponse RunGame(GameInput gameInput, Map map, Dictionary<string, Customer> mapCustomerLookup)
        {
            List<Customer> customerList = RequestCustomers(gameInput, map, mapCustomerLookup);

            budget = map.Budget;

            double num = customerList.Sum<Customer>((Func<Customer, double>)(c => c.Loan.Amount));
            budget -= num;

            HandleIterations(gameInput.Iterations, customerList, map);
            GameResult gameResult = SaveGame(gameInput, customerList);
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

                if (customer.Propose(proposal.YearlyInterestRate, proposal.MonthsToPayBackLoan, this.personalities))
                    acceptedCustomers.Add(customer);
            }
            return acceptedCustomers;
        }

        private void HandleIterations(
          List<Dictionary<string, CustomerAction>> iterations,
          List<Customer> customers,
          Map map)
        {
            for (int index = 0; index < iterations.Count; ++index)
            {
                // ISSUE: reference to a compiler-generated field
                CollectPayments(iterations[index], index, customers, map);
            }
        }

        public void CollectPayments(
          Dictionary<string, CustomerAction> iteration,
          int iterationIndex,
          List<Customer> customers,
          Map map)
        {
            foreach (Customer customer in customers)
            {
                CustomerAction customerAction = iteration[customer.Name];
                customer.Payday();
                customer.PayBills(iterationIndex, personalities);
                if (customer.CanPayLoan())
                    budget += customer.PayLoan();
                else
                    customer.IncrementMark();

                //TODO this is a performance bottleneck to do this every time. Should be done once in the beginning. Or the gameinput should have a type for it. 
                //TODO maybe I should change the POCO to have the enum instead. 
                if (!Enum.TryParse(customerAction.Type, out CustomerActionType actionType))
                    throw new Exception($"Can't find matching enum for customer action {customerAction.Type}.");

                if (actionType == CustomerActionType.Award)
                {
                    //TODO another performance bottleneck. 
                    if (!Enum.TryParse(customerAction.Award, out AwardType awardType))
                        throw new Exception($"Can't find matching enum for award {customerAction.Award}.");

                    budget -= Award(customer, awardType);
                }
            }
        }

        private double Award(Customer customer, AwardType award)
        {

            switch (award)
            {
                case AwardType.IkeaCheck:
                    AwardSpecification award1 = awards[AwardType.IkeaCheck];
                    customer.Happiness += award1.BaseHappiness * personalities[customer.Personality].HappinessMultiplier;
                    return award1.Cost;
                case AwardType.IkeaFoodCoupon:
                    AwardSpecification award2 = awards[AwardType.IkeaFoodCoupon];
                    customer.Happiness += award2.BaseHappiness * personalities[customer.Personality].HappinessMultiplier;
                    return award2.Cost;
                case AwardType.IkeaDeliveryCheck:
                    AwardSpecification award3 = awards[AwardType.IkeaDeliveryCheck];
                    customer.Happiness += award3.BaseHappiness * personalities[customer.Personality].HappinessMultiplier;
                    return award3.Cost;
                case AwardType.NoInterestRate:
                    AwardSpecification award4 = awards[AwardType.NoInterestRate];
                    customer.Happiness += award4.BaseHappiness * personalities[customer.Personality].HappinessMultiplier;
                    return customer.Loan.GetInterestPayment();
                case AwardType.GiftCard:
                    AwardSpecification award5 = awards[AwardType.GiftCard];
                    customer.Happiness += award5.BaseHappiness * personalities[customer.Personality].HappinessMultiplier;
                    return award5.Cost;
                case AwardType.HalfInterestRate:
                    AwardSpecification award6 = awards[AwardType.HalfInterestRate];
                    customer.Happiness += award6.BaseHappiness * personalities[customer.Personality].HappinessMultiplier;
                    return customer.Loan.GetInterestPayment() / 2.0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(award), (object)award, (string)null);
            }
         }

        public GameResult SaveGame(
            GameInput gameInput,
            List<Customer> customers)
        {
            GameResult gameResult = new GameResult()
            {
                TotalProfit = customers.Sum<Customer>((Func<Customer, double>)(x => x.Profit)),
                HappynessScore = customers.Sum<Customer>((Func<Customer, double>)(x => x.Happiness)),
                EnvironmentalImpact = customers.Sum<Customer>((Func<Customer, double>)(x => x.Loan.EnvironmentalImpact)),
                MapName = gameInput.MapName
            };
            return gameResult;
        }
    }
}