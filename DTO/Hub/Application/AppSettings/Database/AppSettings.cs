using DTO.General.Base.Database;
using DTO.Hub.Application.AppSettings.Input;
using DTO.Mobile.Surf.Enum;
using DTO.Surf.Enum;
using System.Collections.Generic;

namespace DTO.Hub.Application.AppSettings.Database
{
    public class AppSettings : BaseData
    {
        public AppSettings(HubAppSettingsInput input)
        {
            if (input == null)
                return;

            MainColor = input.MainColor;
            AllyId = input.AllyId;
            Celular = input.Celular;
            Visao360 = input.Visao360;
            TV = input.TV;
            SupportConfigs = input.SupportConfigs;
            Type = input.Type;
            Tools = input.Tools;
        }

        public AppSettings(string id, HubAppSettingsInput input)
        {
            if (input == null)
                return;

            Id = id;
            MainColor = input.MainColor;
            AllyId = input.AllyId;
            Celular = input.Celular;
            Visao360 = input.Visao360;
            TV = input.TV;
            SupportConfigs = input.SupportConfigs;
            Type = input.Type;
            Tools = input.Tools;
        }

        public bool DisableLogin { get; set; }
        public string AllyId { get; set; }
        public string MainColor { get; set; }
        public AppSegmentsConfigs TV { get; set; }
        public AppSegmentsConfigs Visao360 { get; set; }
        public AppSegmentsConfigs Celular { get; set; }
        public SupportConfigs SupportConfigs { get; set; }
        public AppPartnerTypeEnum Type { get; set; }
        public AppToolsEnum Tools { get; set; }
    }

    public class Colors
    {
        public string Text { get; set; }
        public string TextDarker { get; set; }
        public string Main { get; set; }
        public string Background { get; set; }
        public string BackgroundLighter { get; set; }
        public string BackgroundShadow { get; set; }
    }

    public class AppSegmentsConfigs
    {
        public Colors Colors { get; set; }
    }

    public class SupportConfigs
    {
        public AppSupportOptionsEnum Options { get; set; }
        public TextConfig Email { get; set; }
        public NumberConfig Whatsapp { get; set; }
        public NumberConfig Phone { get; set; }
        public Subsidiary Subsidiary { get; set; }
    }

    public class SubsidiaryConfig
    {
        public string Name { get; set; }
        public string AddressLabel { get; set; }
        public TextConfig Email { get; set; }
        public NumberConfig Whatsapp { get; set; }
        public NumberConfig Phone { get; set; }
    }

    public class BaseConfig
    {
        public string Label { get; set; }
    }

    public class TextConfig : BaseConfig
    {
        public string Data { get; set; }
    }

    public class NumberConfig : BaseConfig
    {
        public string Number { get; set; }
    }

    public class Subsidiary : BaseConfig
    {
        public List<SubsidiaryConfig> Configs { get; set; }
    }
}
