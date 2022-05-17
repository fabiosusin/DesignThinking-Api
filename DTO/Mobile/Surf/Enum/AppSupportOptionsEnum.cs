using System;

namespace DTO.Surf.Enum
{
    [Flags]
    public enum AppSupportOptionsEnum
    {
        Unknown = 0,
        Whatsapp = 1 << 0,
        Phone = 1 << 1,
        Email = 1 << 2,
        Subsidiaries = 1 << 3
    }
}
