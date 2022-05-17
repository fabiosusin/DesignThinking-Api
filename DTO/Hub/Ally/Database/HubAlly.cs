using DTO.General.Address.Input;
using DTO.General.Base.Database;
using DTO.Hub.Ally.Enum;

namespace DTO.Hub.Ally.Database
{
    public class HubAlly : BaseData
    {
        public long Code { get; set; }
        public bool Active { get; set; }
        public bool IsMasterAlly { get; set; }
        public string Phone { get; set; }
        public string SigeId { get; set; }
        public string IE { get; set; }
        public string Name { get; set; }
        public string CorporateName { get; set; }
        public string ThirdPart { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string AccountPlanId { get; set; }
        public string RecurrenceAccountPlanId { get; set; }
        public string ExpenseAccountPlanId { get; set; }
        public HubAllyChargeTypeEnum ChargeType { get; set; }
        public Address Address { get; set; }
    }
}
