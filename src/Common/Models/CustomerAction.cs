
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
