// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Map
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace optimizer.Models.Simulation
{
    public record Map
    {
        public string Name { get; init; }

        public double Budget { get; set; }

        public int GameLengthInMonths { get; init; }

        public List<Customer> Customers { get; init; }

        public Map()
        {
        }
    }
}
