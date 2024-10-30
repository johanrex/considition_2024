// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.ISaveGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37D09AE0-70E5-46F8-B3D7-80D789257673
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace LocalHost.Interfaces
{
  public interface ISaveGameService
  {
    Task<GameResult> SaveGame(
      GameInput gameInput,
      List<Customer> customers,
      Guid apiKey,
      Guid gameId);

    Task<LocalHost.Models.SaveGame?> GetGame(Guid gameId, Guid apiKey);
  }
}
