// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.CustomerLoanRequestProposal
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1678F578-689D-4062-BED4-DD7ABDE09D6A
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record CustomerLoanRequestProposal()
    {
        public string CustomerName { get; set; }

        public double YearlyInterestRate { get; set; }

        public int MonthsToPayBackLoan { get; set; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("CustomerName = ");
            builder.Append((object)this.CustomerName);
            builder.Append(", YearlyInterestRate = ");
            builder.Append(this.YearlyInterestRate.ToString());
            builder.Append(", MonthsToPayBackLoan = ");
            builder.Append(this.MonthsToPayBackLoan.ToString());
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.\u003CCustomerName\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.\u003CYearlyInterestRate\u003Ek__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.\u003CMonthsToPayBackLoan\u003Ek__BackingField);
        }

        [CompilerGenerated]
        public virtual bool Equals(CustomerLoanRequestProposal? other)
        {
            if ((object)this == (object)other)
                return true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (object)other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.\u003CCustomerName\u003Ek__BackingField, other.\u003CCustomerName\u003Ek__BackingField) && EqualityComparer<double>.Default.Equals(this.\u003CYearlyInterestRate\u003Ek__BackingField, other.\u003CYearlyInterestRate\u003Ek__BackingField) && EqualityComparer<int>.Default.Equals(this.\u003CMonthsToPayBackLoan\u003Ek__BackingField, other.\u003CMonthsToPayBackLoan\u003Ek__BackingField);
        }

        [CompilerGenerated]
        protected CustomerLoanRequestProposal(CustomerLoanRequestProposal original)
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CCustomerName\u003Ek__BackingField = original.\u003CCustomerName\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CYearlyInterestRate\u003Ek__BackingField = original.\u003CYearlyInterestRate\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CMonthsToPayBackLoan\u003Ek__BackingField = original.\u003CMonthsToPayBackLoan\u003Ek__BackingField;
        }
    }
}
