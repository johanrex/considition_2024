// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.ICustomerService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 77EDA3FC-B32E-487F-8161-20E228F5089F
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Models;
using System.Collections.Generic;

#nullable enable
namespace LocalHost.Interfaces
{
    public interface ICustomerService
    {
        List<Customer> RequestCustomers(GameInput gameInput, Map map);
    }
}
