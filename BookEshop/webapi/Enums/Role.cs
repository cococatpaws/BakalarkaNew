using System.ComponentModel;

namespace webapi.Enums
{
    public enum Role
    {
        [Description("Admin")]
        Admin,
        [Description("Používateľ")]
        User,
    }
}
