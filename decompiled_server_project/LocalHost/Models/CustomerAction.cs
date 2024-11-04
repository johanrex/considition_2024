// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.CustomerAction
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1678F578-689D-4062-BED4-DD7ABDE09D6A
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace LocalHost.Models
{
    public record CustomerAction()
    {
        public CustomerActionType Type { get; set; }

        public AwardType Award { get; set; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Type = ");
            builder.Append(this.Type.ToString());
            builder.Append(", Award = ");
            builder.Append(this.Award.ToString());
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (EqualityComparer<System.Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<CustomerActionType>.Default.GetHashCode(this.\u003CType\u003Ek__BackingField)) * -1521134295 + EqualityComparer<AwardType>.Default.GetHashCode(this.\u003CAward\u003Ek__BackingField);
        }

        [CompilerGenerated]
        public virtual bool Equals(CustomerAction? other)
        {
            if ((object)this == (object)other)
                return true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (object)other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<CustomerActionType>.Default.Equals(this.\u003CType\u003Ek__BackingField, other.\u003CType\u003Ek__BackingField) && EqualityComparer<AwardType>.Default.Equals(this.\u003CAward\u003Ek__BackingField, other.\u003CAward\u003Ek__BackingField);
        }

        [CompilerGenerated]
        protected CustomerAction(CustomerAction original)
        {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CType\u003Ek__BackingField = original.\u003CType\u003Ek__BackingField;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.\u003CAward\u003Ek__BackingField = original.\u003CAward\u003Ek__BackingField;
        }
    }
}
