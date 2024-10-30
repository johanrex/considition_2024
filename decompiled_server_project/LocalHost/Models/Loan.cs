// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Loan
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record Loan()
    {
        private Decimal amount;
        private const int MonthsInAYear = 12;

        public string Product { get; set; }

        public Decimal EnvironmentalImpact { get; set; }

        public Decimal RemainingBalance { get; private set; }

        public Decimal YearlyInterestRate { get; set; }

        public int MonthsToPayBack { get; set; }

        public Decimal Amount
        {
            get => this.amount;
            set
            {
                this.amount = value;
                this.RemainingBalance = value;
            }
        }

        internal void LowerRemainingBalance(Decimal amountToLowerBy)
        {
            if (amountToLowerBy > 0M)
                return;
            if (this.RemainingBalance - amountToLowerBy < 0M)
                this.RemainingBalance = 0M;
            else
                this.RemainingBalance -= amountToLowerBy;
        }

        internal Decimal GetTotalMonthlyPayment()
        {
            Decimal num1 = this.GetMonthlyInterestRate();
            if (num1 == 0M)
                num1 = 0.0001M;
            double x = (double)(1M + num1);
            Decimal num2 = num1 * (Decimal)Math.Pow(x, (double)this.MonthsToPayBack);
            Decimal num3 = (Decimal)Math.Pow(x, (double)this.MonthsToPayBack) - 1M;
            if (num3 == 0M)
                num3 = 0.0001M;
            return this.Amount * (num2 / num3);
        }

        internal Decimal GetInterestPayment() => this.RemainingBalance * this.GetMonthlyInterestRate();

        internal Decimal GetPrincipalPayment()
        {
            return this.GetTotalMonthlyPayment() - this.GetInterestPayment();
        }

        private Decimal GetMonthlyInterestRate() => this.YearlyInterestRate / 12M;

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Product = ");
            builder.Append((object)this.Product);
            builder.Append(", EnvironmentalImpact = ");
            builder.Append(this.EnvironmentalImpact.ToString());
            builder.Append(", RemainingBalance = ");
            builder.Append(this.RemainingBalance.ToString());
            builder.Append(", YearlyInterestRate = ");
            builder.Append(this.YearlyInterestRate.ToString());
            builder.Append(", MonthsToPayBack = ");
            builder.Append(this.MonthsToPayBack.ToString());
            builder.Append(", Amount = ");
            builder.Append(this.Amount.ToString());
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.\u003CProduct\u003Ek__BackingField)) * -1521134295 + EqualityComparer<Decimal>.Default.GetHashCode(this.\u003CEnvironmentalImpact\u003Ek__BackingField)) * -1521134295 + EqualityComparer<Decimal>.Default.GetHashCode(this.\u003CRemainingBalance\u003Ek__BackingField)) * -1521134295 + EqualityComparer<Decimal>.Default.GetHashCode(this.\u003CYearlyInterestRate\u003Ek__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.\u003CMonthsToPayBack\u003Ek__BackingField)) * -1521134295 + EqualityComparer<Decimal>.Default.GetHashCode(this.amount);
        }

        [CompilerGenerated]
        public virtual bool Equals(Loan? other)
        {
            if ((object)this == (object)other)
                return true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (object)other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.\u003CProduct\u003Ek__BackingField, other.\u003CProduct\u003Ek__BackingField) && EqualityComparer<Decimal>.Default.Equals(this.\u003CEnvironmentalImpact\u003Ek__BackingField, other.\u003CEnvironmentalImpact\u003Ek__BackingField) && EqualityComparer<Decimal>.Default.Equals(this.\u003CRemainingBalance\u003Ek__BackingField, other.\u003CRemainingBalance\u003Ek__BackingField) && EqualityComparer<Decimal>.Default.Equals(this.\u003CYearlyInterestRate\u003Ek__BackingField, other.\u003CYearlyInterestRate\u003Ek__BackingField) && EqualityComparer<int>.Default.Equals(this.\u003CMonthsToPayBack\u003Ek__BackingField, other.\u003CMonthsToPayBack\u003Ek__BackingField) && EqualityComparer<Decimal>.Default.Equals(this.amount, other.amount);
        }

        [CompilerGenerated]
        protected Loan(Loan original)
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CProduct\u003Ek__BackingField = original.\u003CProduct\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CEnvironmentalImpact\u003Ek__BackingField = original.\u003CEnvironmentalImpact\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CRemainingBalance\u003Ek__BackingField = original.\u003CRemainingBalance\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CYearlyInterestRate\u003Ek__BackingField = original.\u003CYearlyInterestRate\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CMonthsToPayBack\u003Ek__BackingField = original.\u003CMonthsToPayBack\u003Ek__BackingField;
            this.amount = original.amount;
        }
    }
}
