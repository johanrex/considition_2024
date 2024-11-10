// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Customer
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D1B7BF3C-328E-422C-8A9F-0E1266BF8FE0
// Assembly location: C:\temp\app\LocalHost.dll

using Common.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace Common.Models
{
    public record Customer
    {
        public string Name { get; init; }

        public string Gender { get; set; }

        public Loan Loan { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Personality Personality { get; init; }

        public double Capital { get; set; }

        public double Income { get; init; }

        public double MonthlyExpenses { get; init; }

        public int NumberOfKids { get; init; }

        public double HomeMortgage { get; init; }

        public bool HasStudentLoan { get; init; }

        public double Happiness { get; set; }

        public bool IsBankrupt { get; private set; }

        public int Marks { get; private set; }

        public int AwardsInRow { get; set; }

        public List<AwardType> AwardsReceived { get; set; } = new List<AwardType>();

        public int MonthsWithoutAwardsInRow { get; set; }

        public double Profit { get; set; }

        private int MarkLimit { get; } = 3;

        public void Payday() => this.Capital += this.Income;

        public void PayBills(
          int iteration,
          Dictionary<Personality, PersonalitySpecification> personalityDict)
        {
            bool flag = iteration % 3 == 0;
            this.Capital -= this.MonthlyExpenses * personalityDict[this.Personality].LivingStandardMultiplier + (this.HasStudentLoan & flag ? 2000.0 : 0.0) + (double)(this.NumberOfKids * 2000) + this.HomeMortgage * 0.001;
        }

        public bool CanPayLoan() => this.Capital >= this.Loan.GetTotalMonthlyPayment();

        public double PayLoan()
        {
            this.Capital -= this.Loan.GetTotalMonthlyPayment();
            double interestPayment = this.Loan.GetInterestPayment();
            this.Loan.LowerRemainingBalance();
            this.Profit += interestPayment;
            return interestPayment;
        }

        public void IncrementMark()
        {
            ++this.Marks;
            if (this.Marks >= this.MarkLimit)
            {
                this.IsBankrupt = true;
                this.Happiness = -500.0;
            }
            else
                this.Happiness -= 50.0;
        }

        public bool Propose(double yearlyInterestRate, int monthsToPayBack, Dictionary<Personality, PersonalitySpecification> personalityDict, int mapLength)
        {
            double? acceptedMinInterest = personalityDict[Personality].AcceptedMinInterest;
            double? acceptedMaxInterest = personalityDict[Personality].AcceptedMaxInterest;
            if (yearlyInterestRate < acceptedMinInterest || yearlyInterestRate > acceptedMaxInterest)
            {
                return false;
            }
            int t = (int)(1 + Personality);
            if (t * mapLength < monthsToPayBack)
            {
                return false;
            }
            Loan.YearlyInterestRate = yearlyInterestRate;
            Loan.MonthsToPayBack = monthsToPayBack;
            return true;
        }
    }
}
