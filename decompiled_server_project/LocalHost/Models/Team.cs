// Decompiled with JetBrains decompiler
// Type: Considition.Entities.Team
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1678F578-689D-4062-BED4-DD7ABDE09D6A
// Assembly location: C:\temp\app\LocalHost.dll

using System;

#nullable enable
namespace Considition.Entities
{
    public class Team
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Venue { get; set; }

        public Guid ApiKey { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
