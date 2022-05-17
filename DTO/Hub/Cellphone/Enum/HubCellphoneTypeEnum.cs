using System.ComponentModel;

namespace DTO.Hub.Cellphone.Enum
{
    public enum HubCellphoneTypeEnum
    {
        [Description("Unknown")]
        Unknown,
        [Description("Pré-Pago")]
        Prepaid,
        [Description("Pós-Pago")]
        Pospaid
    }
}
