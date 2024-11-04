// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.SaveGame
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1678F578-689D-4062-BED4-DD7ABDE09D6A
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;

#nullable enable
namespace LocalHost.Models
{
    public record SaveGame(GameInput GameInput, List<Customer> Customers, GameResult GameResult);
}
