﻿// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.IIterationService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37D09AE0-70E5-46F8-B3D7-80D789257673
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Models;
using System.Collections.Generic;

#nullable enable
namespace LocalHost.Interfaces
{
    public interface IIterationService
    {
        void CollectPayments(
          CustomerActionIteration iteration,
          int i,
          List<Customer> customers,
          Map map);
    }
}
