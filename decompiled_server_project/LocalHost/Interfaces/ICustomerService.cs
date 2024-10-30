// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.ICustomerService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
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
