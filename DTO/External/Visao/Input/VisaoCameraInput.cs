using DTO.External.Visao.Database;
using DTO.External.Visao.Enum;
using System;
using System.Collections.Generic;

namespace DTO.External.Visao.Input
{
    public class VisaoCameraInput
    {
        public string Name { get; set; }
        public string CameraImgBase64 { get; set; }
        public string CameraImgLink { get; set; }
        public string CameraLink { get; set; }
        public string AllyId { get; set; }
        public bool FreeAccess { get; set; }
        public bool Featured { get; set; }
        public VisaoCameraStatusEnum Status { get; set; }
        public DateTime RegisterDate { get; set; }
        public CameraAddress Address { get; set; }
        public List<PartnerDataInput> Partners { get; set; }
    }

    public class PartnerDataInput
    {
        public string LogoBase64 { get; set; }
        public string LogoLink { get; set; }
        public string LogoInlineBase64 { get; set; }
        public string LogoInlineLink { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PartnerSupportConfigs Support { get; set; }
        public VisaoCameraPartnerType Type { get; set; }
    }
}
