// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.ISaveGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79D8B4B1-4F4D-4A0C-BFF7-A27C4AB10C69
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
        Task SaveGame(
          GameInput gameInput,
          GameResult gameResult,
          List<Customer> customers,
          Guid apiKey,
          Guid gameId);

        Task<LocalHost.Models.SaveGame?> GetGame(Guid gameId, Guid apiKey);
    }
}
