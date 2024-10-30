// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Customer
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1790A9F3-C8FD-4294-9282-EE084D3CC633
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace LocalHost.Models
{
    public record Customer
    {
        public string Name { get; init; }

        public Loan Loan { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Personality Personality { get; init; }

        public double Capital { get; set; }

        public double Income { get; init; }

        public double MonthlyExpenses { get; init; }

        public int NumberOfKids { get; init; }

        public double Mortgage { get; init; }

        public bool HasStudentLoans { get; init; }

        public double Happiness { get; set; }

        public bool IsBankrupt { get; private set; }

        public int Marks { get; private set; }

        public int SuccessfulPaymentStreak { get; set; }

        public double Profit { get; private set; }

        private int MarkLimit { get; }

        public void Payday() => this.Capital += this.Income;

        public void PayBills(
          int iteration,
          Dictionary<Personality, PersonalitySpecification> personalityDict)
        {
            bool flag = iteration % 3 == 0;
            this.Capital -= this.MonthlyExpenses * personalityDict[this.Personality].LivingStandardMultiplier - (this.HasStudentLoans & flag ? 2000.0 : 0.0) - (double)(this.NumberOfKids * 2000) - this.Mortgage * 0.01;
        }

        public bool CanPayLoan() => this.Capital >= this.Loan.GetTotalMonthlyPayment();

        public double PayLoan()
        {
            this.Capital -= this.Loan.GetTotalMonthlyPayment();
            this.Loan.LowerRemainingBalance(this.Loan.GetPrincipalPayment());
            ++this.SuccessfulPaymentStreak;
            double interestPayment = this.Loan.GetInterestPayment();
            this.Profit += interestPayment;
            return interestPayment;
        }

        public void IncrementMark()
        {
            ++this.Marks;
            this.SuccessfulPaymentStreak = 0;
            if (this.Marks >= this.MarkLimit)
            {
                this.IsBankrupt = true;
                this.Happiness -= 500.0;
            }
            else
                this.Happiness -= 50.0;
        }

        public bool Propose(
          double yearlyInterestRate,
          int monthsToPayBack,
          Dictionary<Personality, PersonalitySpecification> personalityDict)
        {
            double? acceptedMinInterest = personalityDict[this.Personality].AcceptedMinInterest;
            double? acceptedMaxInterest = personalityDict[this.Personality].AcceptedMaxInterest;
            double num1 = yearlyInterestRate;
            double? nullable1 = acceptedMinInterest;
            double valueOrDefault1 = nullable1.GetValueOrDefault();
            if (!(num1 < valueOrDefault1 & nullable1.HasValue))
            {
                double num2 = yearlyInterestRate;
                double? nullable2 = acceptedMaxInterest;
                double valueOrDefault2 = nullable2.GetValueOrDefault();
                if (!(num2 > valueOrDefault2 & nullable2.HasValue))
                {
                    this.Loan.YearlyInterestRate = yearlyInterestRate;
                    this.Loan.MonthsToPayBack = monthsToPayBack;
                    return true;
                }
            }
            return false;
        }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Name = ");
            builder.Append((object)this.Name);
            builder.Append(", Loan = ");
            builder.Append((object)this.Loan);
            builder.Append(", Personality = ");
            builder.Append(this.Personality.ToString());
            builder.Append(", Capital = ");
            builder.Append(this.Capital.ToString());
            builder.Append(", Income = ");
            builder.Append(this.Income.ToString());
            builder.Append(", MonthlyExpenses = ");
            builder.Append(this.MonthlyExpenses.ToString());
            builder.Append(", NumberOfKids = ");
            builder.Append(this.NumberOfKids.ToString());
            builder.Append(", Mortgage = ");
            builder.Append(this.Mortgage.ToString());
            builder.Append(", HasStudentLoans = ");
            builder.Append(this.HasStudentLoans.ToString());
            builder.Append(", Happiness = ");
            builder.Append(this.Happiness.ToString());
            builder.Append(", IsBankrupt = ");
            builder.Append(this.IsBankrupt.ToString());
            builder.Append(", Marks = ");
            builder.Append(this.Marks.ToString());
            builder.Append(", SuccessfulPaymentStreak = ");
            builder.Append(this.SuccessfulPaymentStreak.ToString());
            builder.Append(", Profit = ");
            builder.Append(this.Profit.ToString());
            return true;
        }

        public Customer()
        {
        }
    }
}
