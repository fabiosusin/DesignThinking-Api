using System.ComponentModel;

namespace DTO.Hub.Ally.Enum
{
    public enum HubIcmsTypeEnum
    {
        [Description("Unknown")]
        Unknown,
        [Description("Contribuinte")]
        Taxpayer,
        [Description("Isento")]
        Free,
        [Description("Não Contribuinte")]
        NotTaxpayer
    }
}