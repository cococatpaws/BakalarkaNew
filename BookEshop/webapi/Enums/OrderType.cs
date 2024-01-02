using System.ComponentModel;

namespace webapi.Enums
{
    public enum OrderType
    {
        [Description("Domov")]
        HomeDeliver,
        [Description("Rezervácia na predajni")]
        StorePickup
    }
}
