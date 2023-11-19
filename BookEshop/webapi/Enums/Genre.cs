using System;
using System.ComponentModel;

namespace webapi.Enums
{
    public enum Genre
    {
        [Description("Detektívka")]
        Detective,
        [Description("Triler")]
        Thriller,
        [Description("Horor")]
        Horror,
        [Description("Komiks")]
        Comics,
        [Description("Romantika")]
        Romance,
        [Description("Klasika")]
        Classics,
        [Description("Historická")]
        Historical,
        [Description("Poézia")]
        Poetry,
        [Description("Fantasy")]
        Fantasy,
        [Description("Dráma")]
        Drama,
        [Description("Scifi")]
        SciFi,
        [Description("Náučná")]
        Educational,
        [Description("Motivačná")]
        Motivational,
        [Description("Učebnica")]
        Textbook,
        [Description("Biografia")]
        Biography
    }
}