// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Customer
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
// Assembly location: C:\temp\app\LocalHost.dll

using System;
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

        public Decimal Capital { get; set; }

        public Decimal Income { get; init; }

        public Decimal MonthlyExpenses { get; init; }

        public int NumberOfKids { get; init; }

        public Decimal Mortgage { get; init; }

        public bool HasStudentLoans { get; init; }

        public Decimal Happiness { get; set; }

        public bool IsBankrupt { get; private set; }

        public int Marks { get; private set; }

        public int SuccessfulPaymentStreak { get; set; }

        public Decimal Profit { get; private set; }

        private int MarkLimit { get; }

        public void Payday() => this.Capital += this.Income;

        public void PayBills(
          int iteration,
          Dictionary<Personality, PersonalitySpecification> personalityDict)
        {
            bool flag = iteration % 3 == 0;
            this.Capital -= this.MonthlyExpenses * personalityDict[this.Personality].LivingStandardMultiplier - (Decimal)(this.HasStudentLoans & flag ? 2000 : 0) - (Decimal)(this.NumberOfKids * 2000) - this.Mortgage * 0.01M;
        }

        public bool CanPayLoan() => this.Capital >= this.Loan.GetTotalMonthlyPayment();

        public Decimal PayLoan()
        {
            this.Capital -= this.Loan.GetTotalMonthlyPayment();
            this.Loan.LowerRemainingBalance(this.Loan.GetPrincipalPayment());
            ++this.SuccessfulPaymentStreak;
            Decimal interestPayment = this.Loan.GetInterestPayment();
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
                this.Happiness -= 500.0M;
            }
            else
                this.Happiness -= 50.0M;
        }

        public bool Propose(
          Decimal yearlyInterestRate,
          int monthsToPayBack,
          Dictionary<Personality, PersonalitySpecification> personalityDict)
        {
            Decimal? acceptedMinInterest = personalityDict[this.Personality].AcceptedMinInterest;
            Decimal? acceptedMaxInterest = personalityDict[this.Personality].AcceptedMaxInterest;
            Decimal num1 = yearlyInterestRate;
            Decimal? nullable1 = acceptedMinInterest;
            Decimal valueOrDefault1 = nullable1.GetValueOrDefault();
            if (!(num1 < valueOrDefault1 & nullable1.HasValue))
            {
                Decimal num2 = yearlyInterestRate;
                Decimal? nullable2 = acceptedMaxInterest;
                Decimal valueOrDefault2 = nullable2.GetValueOrDefault();
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
