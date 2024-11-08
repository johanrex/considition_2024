// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.IGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 77EDA3FC-B32E-487F-8161-20E228F5089F
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
