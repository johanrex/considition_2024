// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.SaveGame
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 277A783F-1186-461D-9163-D01AAF05EBE1
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;

#nullable enable
namespace LocalHost.Models
{
    public record SaveGame(GameInput GameInput, List<Customer> Customers, GameResult GameResult);
}
