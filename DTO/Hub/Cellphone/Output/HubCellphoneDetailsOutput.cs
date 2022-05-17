using DTO.General.Base.Api.Output;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Customer.Database;
using DTO.Mobile.Surf.Database;

namespace DTO.Hub.Cellphone.Output
{
    public class HubCellphoneDetailsOutput : BaseApiOutput
    {
        public HubCellphoneDetailsOutput(string msg) : base(msg) { }
        public HubCellphoneDetailsOutput(HubCellphoneManagement management, HubCustomer customer, SurfMobilePlan surfPlan) : base(true) => CellphoneDetails = new(management, customer, surfPlan);
        public HubCellphoneDetails CellphoneDetails { get; set; }
    }

    public class HubCellphoneDetails
    {
        public HubCellphoneDetails(HubCellphoneManagement management, HubCustomer customer, SurfMobilePlan surfPlan)
        {
            if (management == null)
                return;

            Number = !string.IsNullOrEmpty(management.CellphoneData?.Number) && !string.IsNullOrEmpty(management.CellphoneData?.DDD) ?
                management.CellphoneData.DDD + management.CellphoneData.Number : !string.IsNullOrEmpty(management.CellphoneData?.Number) ? management.CellphoneData.Number : null;
            ManagementId = management.Id;
            SurfTransactionId = management.SurfTransactionId;
            Mode = management.Mode;
            Status = management.Status;
            ChipSerial = management.ChipSerial;
            Portability = new(management.Portability?.OperatorId, management.Portability?.Number);
            Price = management.Price;

            if (customer != null)
                Customer = new(customer.Id, customer.Name);

            if (surfPlan != null)
                SurfPlan = new(surfPlan.Id, surfPlan.Name);
        }

        public string Number { get; set; }
        public string ManagementId { get; set; }
        public string SurfTransactionId { get; set; }
        public string ChipSerial { get; set; }
        public HubCellphoneManagementStatusEnum Status { get; set; }
        public HubCellphoneManagementTypeEnum Mode { get; set; }
        public HubCellphoneDetailsData Customer { get; set; }
        public HubCellphoneDetailsData SurfPlan { get; set; }
        public HubCellphonePortabilityDetails Portability { get; set; }
        public CellphoneManagementPrice Price { get; set; }
    }

    public class HubCellphoneDetailsData
    {
        public HubCellphoneDetailsData(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class HubCellphonePortabilityDetails
    {
        public HubCellphonePortabilityDetails(string operatorId, string originNumber)
        {
            OriginNumber = originNumber;
            OperatorId = operatorId;
        }
        public string OperatorId { get; set; }
        public string OriginNumber { get; set; }
    }
}
