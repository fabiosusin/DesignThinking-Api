using System.ComponentModel;

namespace DTO.Surf.Enum
{
    public enum AppCallDataTypeEnum
    {
        [Description("Unknown")]
        Unknown,
        [Description("VOICE")]
        Voice,
        [Description("SMS")]
        Sms,
        [Description("DATA")]
        Data
    }
}