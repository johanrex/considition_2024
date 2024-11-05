using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

#nullable disable
namespace Common.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AwardType
    {
        None,
        IkeaCheck,
        IkeaFoodCoupon,
        IkeaDeliveryCheck,
        NoInterestRate,
        GiftCard,
        HalfInterestRate,
    }
}
