using DTO.External.Visao.Enum;
using DTO.External.Visao.Input;
using DTO.General.Base.Database;
using DTO.Hub.Application.AppSettings.Database;
using System;
using System.Collections.Generic;

namespace DTO.External.Visao.Database
{
    public class VisaoCamera : BaseData
    {
        public VisaoCamera() { }
        public VisaoCamera(VisaoCameraInput input)
        {
            if (input == null)
                return;

            Name = input.Name;
            CameraLink = input.CameraLink;
            AllyId = input.AllyId;
            FreeAccess = input.FreeAccess;
            Address = input.Address;
            Featured = input.Featured;
            RegisterDate = input.RegisterDate == DateTime.MinValue ? DateTime.Now : input.RegisterDate;
            Status = input.Status == VisaoCameraStatusEnum.Unknown ? VisaoCameraStatusEnum.Active : input.Status;
            Partners = new List<PartnerData>();
        }

        public VisaoCamera(string id, string camImg, List<PartnerData> partners, VisaoCameraInput input)
        {
            Id = id;
            if (input == null)
                return;

            Name = input.Name;
            CameraImg = camImg;
            CameraLink = input.CameraLink;
            AllyId = input.AllyId;
            FreeAccess = input.FreeAccess;
            Address = input.Address;
            Featured = input.Featured;
            RegisterDate = input.RegisterDate == DateTime.MinValue ? DateTime.Now : input.RegisterDate;
            Status = input.Status == VisaoCameraStatusEnum.Unknown ? VisaoCameraStatusEnum.Active : input.Status;
            Partners = partners;
        }


        public string Name { get; set; }
        public string CameraLink { get; set; }
        public string CameraImg { get; set; }
        public string AllyId { get; set; }
        public bool FreeAccess { get; set; }
        public bool Featured { get; set; }
        public VisaoCameraStatusEnum Status { get; set; }
        public DateTime RegisterDate { get; set; }
        public CameraAddress Address { get; set; }
        public List<PartnerData> Partners { get; set; }
    }

    public class PartnerData
    {
        public PartnerData(string name, string logo, string logoInline, string description, PartnerSupportConfigs configs, VisaoCameraPartnerType type)
        {
            Type = type;
            Logo = logo;
            LogoInline = logoInline;
            Name = name;
            Description = description;
            Support = configs;
        }

        public string Logo { get; set; }
        public string LogoInline { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PartnerSupportConfigs Support { get; set; }
        public VisaoCameraPartnerType Type { get; set; }
    }

    public class CameraAddress
    {
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class PartnerSupportConfigs
    {
        public TextConfig Email { get; set; }
        public NumberConfig Whatsapp { get; set; }
        public NumberConfig Phone { get; set; }
    }
}
