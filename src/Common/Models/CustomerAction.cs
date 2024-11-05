// Decompiled with JetBrains decompiler
// Type: LocalHost.Models.CustomerAction
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AA0D6786-29C9-4DD4-9CA6-D5CCB27ABAAB
// Assembly location: C:\temp\app\LocalHost.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace Common.Models
{
    public record CustomerAction()
    {
        public CustomerActionType Type { get; set; }

        public AwardType Award { get; set; }

    }
}
