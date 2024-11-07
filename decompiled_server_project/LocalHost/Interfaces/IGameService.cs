// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.IGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D1B7BF3C-328E-422C-8A9F-0E1266BF8FE0
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Models;
using System;
using System.Threading.Tasks;

#nullable enable
namespace LocalHost.Interfaces
{
    public interface IGameService
    {
        Task<GameResponse> RunGame(GameInput gameInput, Guid apiKey);

        Task<SaveGame?> GetGame(Guid gameId, Guid apiKey);
    }
}
