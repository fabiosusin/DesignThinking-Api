using DTO.External.Visao.Database;
using DTO.External.Visao.Enum;
using System;
using System.Collections.Generic;

namespace DTO.Mobile.Visao.Output
{
    public class AppCameraOutput
    {
        public AppCameraOutput(VisaoCamera camera)
        {
            if (camera == null)
                return;

            if (camera.Address != null)
                Address = camera.Address;

            Id = camera.Id;
            Name = camera.Name;
            CameraLink = camera.CameraLink;
            CameraImg = camera.CameraImg;
            RegisterDate = camera.RegisterDate;
            Status = camera.Status;
            Partners = camera.Partners;
        }

        public string Id { get; set; }
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
}
