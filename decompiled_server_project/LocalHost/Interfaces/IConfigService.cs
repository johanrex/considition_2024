// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.IConfigService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Models;
using System.Collections.Generic;

#nullable enable
namespace LocalHost.Interfaces
{
    public interface IConfigService
    {
        Map? GetMap(string mapName);

        Dictionary<Personality, PersonalitySpecification> Personalities { get; }

        Dictionary<AwardType, AwardSpecification> Awards { get; }
    }
}
