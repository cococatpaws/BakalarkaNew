using System.ComponentModel;

namespace webapi.Enums
{
    public enum ShippingType
    {
        [Description("Kuriér GLS")]
        GLS,
        [Description("Pošta")]
        Post,
        [Description("Kuriér SPS")]
        SPS,
        [Description("Osobné vyzdvihnutie na predajni")]
        PersonalPickup
    }
}
