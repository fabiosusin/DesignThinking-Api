using DTO.Hub.AccountPlan.Database;
using DTO.Hub.Company.Database;

namespace DTO.Hub.Order.Input
{
    public class HubOrderRegisterSigeOrderInput
    {
        public HubOrderRegisterSigeOrderInput(HubOrderCreationInput input, HubCompany company, HubAccountPlan accountPlan)
        {
            InputOrderCreation = input;
            Company = company;
            AccountPlan = accountPlan;
        }
        public HubOrderCreationInput InputOrderCreation { get; set; }
        public HubCompany Company { get; set; }
        public HubAccountPlan AccountPlan { get; set; }

    }
}
