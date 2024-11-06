// Decompiled with JetBrains decompiler
// Type: LocalHost.Interfaces.IConfigService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E185BA7-B99B-4FD6-9E2E-A742DD973CDE
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Models;
using System.Collections.Generic;

#nullable enable
namespace LocalHost.Interfaces
{
    public interface IConfigService
    {
        Map? GetMap(string mapName);

        Dictionary<Personality, PersonalitySpecification> GetPersonalitySpecifications(string maName);

        Dictionary<AwardType, AwardSpecification> GetAwardSpecifications(string maName);
    }
}
