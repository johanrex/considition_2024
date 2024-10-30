// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.PersonalitySpecification
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1790A9F3-C8FD-4294-9282-EE084D3CC633
// Assembly location: C:\temp\app\LocalHost.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record PersonalitySpecification()
    {
        public double HappinessMultiplier { get; set; }

        public double? AcceptedMinInterest { get; set; }

        public double? AcceptedMaxInterest { get; set; }

        public double LivingStandardMultiplier { get; set; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("HappinessMultiplier = ");
            builder.Append(this.HappinessMultiplier.ToString());
            builder.Append(", AcceptedMinInterest = ");
            builder.Append(this.AcceptedMinInterest.ToString());
            builder.Append(", AcceptedMaxInterest = ");
            builder.Append(this.AcceptedMaxInterest.ToString());
            builder.Append(", LivingStandardMultiplier = ");
            builder.Append(this.LivingStandardMultiplier.ToString());
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.\u003CHappinessMultiplier\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(this.\u003CAcceptedMinInterest\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(this.\u003CAcceptedMaxInterest\u003Ek__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.\u003CLivingStandardMultiplier\u003Ek__BackingField);
        }

        [CompilerGenerated]
        public virtual bool Equals(PersonalitySpecification? other)
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
            return (object)other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<double>.Default.Equals(this.\u003CHappinessMultiplier\u003Ek__BackingField, other.\u003CHappinessMultiplier\u003Ek__BackingField) && EqualityComparer<double?>.Default.Equals(this.\u003CAcceptedMinInterest\u003Ek__BackingField, other.\u003CAcceptedMinInterest\u003Ek__BackingField) && EqualityComparer<double?>.Default.Equals(this.\u003CAcceptedMaxInterest\u003Ek__BackingField, other.\u003CAcceptedMaxInterest\u003Ek__BackingField) && EqualityComparer<double>.Default.Equals(this.\u003CLivingStandardMultiplier\u003Ek__BackingField, other.\u003CLivingStandardMultiplier\u003Ek__BackingField);
        }

        [CompilerGenerated]
        protected PersonalitySpecification(PersonalitySpecification original)
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CHappinessMultiplier\u003Ek__BackingField = original.\u003CHappinessMultiplier\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CAcceptedMinInterest\u003Ek__BackingField = original.\u003CAcceptedMinInterest\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CAcceptedMaxInterest\u003Ek__BackingField = original.\u003CAcceptedMaxInterest\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CLivingStandardMultiplier\u003Ek__BackingField = original.\u003CLivingStandardMultiplier\u003Ek__BackingField;
        }
    }
}
