using System.ComponentModel;

namespace webapi.Enums
{
    public enum PaymentType
    {
        [Description("Dobierka")]
        CashOnDelivery,
        [Description("Prevod na účet")]
        CardTransfer
    }
}
