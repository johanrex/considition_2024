// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Map
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
    public record Map
    {
        public string Name { get; init; }

        public Decimal Budget { get; set; }

        public int GameLengthInMonths { get; init; }

        public List<Customer> Customers { get; init; }

        [CompilerGenerated]
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Name = ");
            builder.Append((object)this.Name);
            builder.Append(", Budget = ");
            builder.Append(this.Budget.ToString());
            builder.Append(", GameLengthInMonths = ");
            builder.Append(this.GameLengthInMonths.ToString());
            builder.Append(", Customers = ");
            builder.Append((object)this.Customers);
            return true;
        }

        public Map()
        {
        }
    }
}
