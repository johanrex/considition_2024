// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.ICustomerService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E185BA7-B99B-4FD6-9E2E-A742DD973CDE
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
