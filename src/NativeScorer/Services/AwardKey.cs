// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.AwardKey
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

#nullable enable
using NativeScorer;
using NativeScorer.Models;

namespace NativeScorer.Services
{
    public record AwardKey(string MapName, AwardType Award);
}
