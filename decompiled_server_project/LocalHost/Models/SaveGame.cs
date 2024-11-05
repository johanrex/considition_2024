// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.SaveGame
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79D8B4B1-4F4D-4A0C-BFF7-A27C4AB10C69
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;

#nullable enable
namespace LocalHost.Models
{
    public record SaveGame(GameInput GameInput, List<Customer> Customers, GameResult GameResult);
}
