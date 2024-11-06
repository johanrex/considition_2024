// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.SaveGame
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E185BA7-B99B-4FD6-9E2E-A742DD973CDE
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;

#nullable enable
namespace LocalHost.Models
{
    public record SaveGame(GameInput GameInput, List<Customer> Customers, GameResult GameResult);
}
