// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.IterationService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D1B7BF3C-328E-422C-8A9F-0E1266BF8FE0
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
            string name = map.Name;
            // ISSUE: reference to a compiler-generated field
            Dictionary<Personality, PersonalitySpecification> personalitySpecifications = this.\u003CconfigService\u003EP.GetPersonalitySpecifications(name);
            // ISSUE: reference to a compiler-generated field
            Dictionary<AwardType, AwardSpecification> awardSpecifications = this.\u003CconfigService\u003EP.GetAwardSpecifications(name);
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
    }
}
