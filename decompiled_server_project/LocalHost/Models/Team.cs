// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.Team
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D1B7BF3C-328E-422C-8A9F-0E1266BF8FE0
// Assembly location: C:\temp\app\LocalHost.dll

using System;

#nullable enable
namespace LocalHost.Models
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
