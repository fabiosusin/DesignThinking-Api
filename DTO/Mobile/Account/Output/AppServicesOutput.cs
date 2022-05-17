using DTO.General.Base.Api.Output;
using DTO.Hub.Cellphone.Database;
using DTO.Mobile.Surf.Database;
using System;
using System.Collections.Generic;

namespace DTO.Mobile.Account.Output
{
    public class AppServicesOutput : BaseApiOutput
    {
        public AppServicesOutput(string msg) : base(msg) { }
        public AppServicesOutput(List<AppServiceDetails> services) : base(true) => Services = services;
        public List<AppServiceDetails> Services { get; set; }
    }

    public class AppServiceDetails
    {
        public AppServiceDetails(SurfMobilePlan plan, HubCellphoneManagement management)
        {
            Title = plan?.Name ?? "Desconhecido";
            if (management == null)
                return;

            Price = management.Price?.OrderPrice ?? 0;
            HiringIn = management.CreationDate;
            Data = "";

            if (!string.IsNullOrEmpty(management.CellphoneData.DDD))
                Data = management.CellphoneData.DDD;

            if (!string.IsNullOrEmpty(management.CellphoneData.Number))
                Data += management.CellphoneData.Number;
        }

        public string Title { get; set; }
        public string Data { get; set; }
        public decimal Price { get; set; }
        public DateTime HiringIn { get; set; }
    }
}
