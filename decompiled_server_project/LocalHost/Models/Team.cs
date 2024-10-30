// Decompiled with JetBrains decompiler
// Type: Considition.Entities.Team
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1790A9F3-C8FD-4294-9282-EE084D3CC633
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
