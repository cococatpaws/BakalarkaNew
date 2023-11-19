using System.ComponentModel;

namespace webapi.Enums
{
    public enum BookLanguage
    {
        [Description("Angličtina")]
        English,
        [Description("Slovenčina")]
        Slovak,
        [Description("Čeština")]
        Czech,
        [Description("Nemčina")]
        German
    }
}
