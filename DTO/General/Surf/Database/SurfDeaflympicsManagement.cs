using DTO.General.Base.Database;
using DTO.Hub.Cellphone.Database;
using DTO.Mobile.Surf.Database;
using System;

namespace DTO.General.Surf.Database
{
    public class SurfDeaflympicsManagement : BaseData
    {
        public SurfDeaflympicsManagement(string allyId, string customer, string surfCustomer, string iccid, string transactionId, SurfMobilePlan plan, SurfCellphoneData cellphoneData)
        {
            AllyId = allyId;
            CustomerId = customer;
            SurfCustomerId = surfCustomer;
            Iccid = iccid;
            CreationDate = DateTime.Now;
            CellphoneData = cellphoneData;
            SurfMobilePlanId = plan?.Id;
            SurfTransactionId = transactionId;

            if (plan?.SurfData?.Price == null)
                return;

            Price = new(plan.SurfData.Price.Cost, plan.SurfData.Price.Price);
            SurfPlanId = plan.SurfData.Id;
        }

        public string AllyId { get; set; }
        public string CustomerId { get; set; }
        public string SurfCustomerId { get; set; }
        public string SurfMobilePlanId { get; set; }
        public string SurfPlanId { get; set; }
        public string SurfTransactionId { get; set; }
        public string Iccid { get; set; }
        public DateTime CreationDate { get; set; }
        public SurfCellphoneData CellphoneData { get; set; }
        public CellphoneManagementPrice Price { get; set; }
    }
}
