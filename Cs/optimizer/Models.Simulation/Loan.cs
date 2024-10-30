
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace optimizer.Models.Simulation
{
    public record Loan()
    {
        private double amount;
        private const int MonthsInAYear = 12;

        public string Product { get; set; }

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

        internal void LowerRemainingBalance(double amountToLowerBy)
        {
            if (amountToLowerBy > 0.0)
                return;
            if (this.RemainingBalance - amountToLowerBy < 0.0)
                this.RemainingBalance = 0.0;
            else
                this.RemainingBalance -= amountToLowerBy;
        }

        internal double GetTotalMonthlyPayment()
        {
            double num1 = this.GetMonthlyInterestRate();
            if (num1 == 0.0)
                num1 = 0.0001;
            double x = 1.0 + num1;
            double num2 = num1 * Math.Pow(x, (double)this.MonthsToPayBack);
            double num3 = Math.Pow(x, (double)this.MonthsToPayBack) - 1.0;
            if (num3 == 0.0)
                num3 = 0.0001;
            return this.Amount * (num2 / num3);
        }

        internal double GetInterestPayment() => this.RemainingBalance * this.GetMonthlyInterestRate();

        internal double GetPrincipalPayment()
        {
            return this.GetTotalMonthlyPayment() - this.GetInterestPayment();
        }

        private double GetMonthlyInterestRate() => this.YearlyInterestRate / 12.0;
    }
}
