
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace Common.Models
{
    public record Loan()
    {
        private double amount;
        private const int MonthsInAYear = 12;

        public required string Product { get; set; }

        public double EnvironmentalImpact { get; set; }

        public double RemainingBalance { get; private set; }

        public double YearlyInterestRate { get; set; }

        public int MonthsToPayBack { get; set; }

        public double Amount
        {
            get => this.amount;
            set
            {
                this.amount = value;
                this.RemainingBalance = value;
            }
        }

        internal void LowerRemainingBalance()
        {
            double principalPayment = this.GetPrincipalPayment();
            if (this.RemainingBalance - principalPayment < 0.0)
                this.RemainingBalance = 0.0;
            else
                this.RemainingBalance -= principalPayment;
        }

        internal double GetTotalMonthlyPayment()
        {
            double interestPayment = this.GetInterestPayment();
            return this.GetPrincipalPayment() + interestPayment;
        }

        public double GetInterestPayment() => this.RemainingBalance * this.GetMonthlyInterestRate();

        public double GetPrincipalPayment() => this.Amount / (double)this.MonthsToPayBack;


        public double GetMonthlyInterestRate() => this.YearlyInterestRate / 12.0;
    }
}
