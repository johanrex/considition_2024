using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace Common.Models
{
    [JsonConverter(typeof(CustomerActionIterationConverter))]
    public record CustomerActionIteration()
    {
        public Dictionary<string, CustomerAction> CustomerActions;
    }
}
