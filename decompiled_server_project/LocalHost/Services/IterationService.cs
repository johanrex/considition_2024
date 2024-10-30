// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.IterationService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Interfaces;
using LocalHost.Models;
using System;
using System.Collections.Generic;

#nullable enable
namespace LocalHost.Services
{
    public class IterationService : IIterationService
    {
        public IterationService(IConfigService configService)
        {
            // ISSUE: reference to a compiler-generated field
            this.\u003CconfigService\u003EP = configService;
            // ISSUE: explicit constructor call
            base.\u002Ector();
        }

        public void CollectPayments(
          CustomerActionIteration iteration,
          int iterationIndex,
          List<Customer> customers,
          Map map)
        {
            foreach (Customer customer in customers)
            {
                CustomerAction customerAction = iteration.CustomerActions[customer.Name];
                customer.Payday();
                // ISSUE: reference to a compiler-generated field
                customer.PayBills(iterationIndex, this.\u003CconfigService\u003EP.Personalities);
                if (customer.CanPayLoan())
                    map.Budget += customer.PayLoan();
                else
                    customer.IncrementMark();
                if (customerAction.Type == CustomerActionType.Award)
                    map.Budget -= this.Award(customer, customerAction.Award);
            }
        }

        private Decimal Award(Customer customer, AwardType award)
        {
            switch (award)
            {
                case AwardType.IkeaCheck:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award1 = this.\u003CconfigService\u003EP.Awards[AwardType.IkeaCheck];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award1.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier;
                    return award1.Cost;
                case AwardType.IkeaFoodCoupon:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award2 = this.\u003CconfigService\u003EP.Awards[AwardType.IkeaFoodCoupon];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award2.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier;
                    return award2.Cost;
                case AwardType.IkeaDeliveryCheck:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award3 = this.\u003CconfigService\u003EP.Awards[AwardType.IkeaDeliveryCheck];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award3.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier;
                    return award3.Cost;
                case AwardType.NoInterestRate:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award4 = this.\u003CconfigService\u003EP.Awards[AwardType.NoInterestRate];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award4.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier;
                    return customer.Loan.GetInterestPayment();
                case AwardType.GiftCard:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award5 = this.\u003CconfigService\u003EP.Awards[AwardType.GiftCard];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award5.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier;
                    return award5.Cost;
                case AwardType.HalfInterestRate:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award6 = this.\u003CconfigService\u003EP.Awards[AwardType.HalfInterestRate];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award6.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier;
                    return customer.Loan.GetInterestPayment() / 2.0M;
                default:
                    throw new ArgumentOutOfRangeException(nameof(award), (object)award, (string)null);
            }
        }
    }
}
