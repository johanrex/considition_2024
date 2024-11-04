// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.IterationService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1678F578-689D-4062-BED4-DD7ABDE09D6A
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

        public string? CollectPayments(
          CustomerActionIteration iteration,
          int month,
          List<Customer> customers,
          Map map)
        {
            foreach (Customer customer in customers)
            {
                if (map.Budget <= 0.0)
                    return "Your bank went bankrupt";
                if (!customer.IsBankrupt)
                {
                    CustomerAction customerAction = iteration.CustomerActions[customer.Name];
                    customer.Payday();
                    // ISSUE: reference to a compiler-generated field
                    customer.PayBills(month, this.\u003CconfigService\u003EP.Personalities);
                    if (customer.CanPayLoan())
                        map.Budget += customer.PayLoan();
                    else
                        customer.IncrementMark();
                    if (customerAction.Type == CustomerActionType.Award)
                    {
                        double num = this.Award(customer, customerAction.Award);
                        customer.Profit -= num;
                        map.Budget -= num;
                    }
                    else if (customer.AwardsInRow > 0)
                        --customer.AwardsInRow;
                }
            }
            return (string)null;
        }

        private double Award(Customer customer, AwardType award)
        {
            double num = Math.Round((100.0 - (double)customer.AwardsInRow * 20.0) / 100.0, 1);
            if (customer.AwardsInRow < 5)
                ++customer.AwardsInRow;
            switch (award)
            {
                case AwardType.IkeaCheck:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award1 = this.\u003CconfigService\u003EP.Awards[AwardType.IkeaCheck];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award1.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier * num;
                    return award1.Cost;
                case AwardType.IkeaFoodCoupon:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award2 = this.\u003CconfigService\u003EP.Awards[AwardType.IkeaFoodCoupon];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award2.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier * num;
                    return award2.Cost;
                case AwardType.IkeaDeliveryCheck:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award3 = this.\u003CconfigService\u003EP.Awards[AwardType.IkeaDeliveryCheck];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award3.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier * num;
                    return award3.Cost;
                case AwardType.NoInterestRate:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award4 = this.\u003CconfigService\u003EP.Awards[AwardType.NoInterestRate];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award4.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier * num;
                    return customer.Loan.GetInterestPayment() + award4.Cost;
                case AwardType.GiftCard:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award5 = this.\u003CconfigService\u003EP.Awards[AwardType.GiftCard];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award5.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier * num;
                    return award5.Cost;
                case AwardType.HalfInterestRate:
                    // ISSUE: reference to a compiler-generated field
                    AwardSpecification award6 = this.\u003CconfigService\u003EP.Awards[AwardType.HalfInterestRate];
                    // ISSUE: reference to a compiler-generated field
                    customer.Happiness += award6.BaseHappiness * this.\u003CconfigService\u003EP.Personalities[customer.Personality].HappinessMultiplier * num;
                    return customer.Loan.GetInterestPayment() / 2.0 + award6.Cost;
                default:
                    throw new ArgumentOutOfRangeException(nameof(award), (object)award, (string)null);
            }
        }
    }
}
