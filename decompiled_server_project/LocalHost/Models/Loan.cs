﻿// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Loan
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37D09AE0-70E5-46F8-B3D7-80D789257673
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
      double num2 = num1 * Math.Pow(x, (double) this.MonthsToPayBack);
      double num3 = Math.Pow(x, (double) this.MonthsToPayBack) - 1.0;
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

    [CompilerGenerated]
    protected virtual bool PrintMembers(StringBuilder builder)
    {
      RuntimeHelpers.EnsureSufficientExecutionStack();
      builder.Append("Product = ");
      builder.Append((object) this.Product);
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
      return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.\u003CProduct\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.\u003CEnvironmentalImpact\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.\u003CRemainingBalance\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.\u003CYearlyInterestRate\u003Ek__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.\u003CMonthsToPayBack\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.amount);
    }

    [CompilerGenerated]
    public virtual bool Equals(Loan? other)
    {
      if ((object) this == (object) other)
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
      return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.\u003CProduct\u003Ek__BackingField, other.\u003CProduct\u003Ek__BackingField) && EqualityComparer<double>.Default.Equals(this.\u003CEnvironmentalImpact\u003Ek__BackingField, other.\u003CEnvironmentalImpact\u003Ek__BackingField) && EqualityComparer<double>.Default.Equals(this.\u003CRemainingBalance\u003Ek__BackingField, other.\u003CRemainingBalance\u003Ek__BackingField) && EqualityComparer<double>.Default.Equals(this.\u003CYearlyInterestRate\u003Ek__BackingField, other.\u003CYearlyInterestRate\u003Ek__BackingField) && EqualityComparer<int>.Default.Equals(this.\u003CMonthsToPayBack\u003Ek__BackingField, other.\u003CMonthsToPayBack\u003Ek__BackingField) && EqualityComparer<double>.Default.Equals(this.amount, other.amount);
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
