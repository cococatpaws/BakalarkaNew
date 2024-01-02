using System.ComponentModel;

namespace webapi.Enums
{
    public enum OrderStatus
    {
        [Description("Objednávka vytvorená")]
        OrderCreated,
        [Description("Objednávka zabalená")]
        OrderPackaged,
        [Description("Objednávka odoslaná")]
        OrderSent,
        [Description("Objednávka dodaná")]
        OrderDelivered
    }
}
