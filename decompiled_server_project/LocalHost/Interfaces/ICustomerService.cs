// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.ICustomerService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDC2938F-C917-4854-87EA-D677106BD5FA
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
