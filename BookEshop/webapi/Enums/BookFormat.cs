using System.ComponentModel;

namespace webapi.Enums
{
    public enum BookFormat
    {
        [Description("Kniha")]
        Book,
        [Description("Audiokniha")]
        Audiobook,
        [Description("E-kniha")]
        Ebook
    }
}
