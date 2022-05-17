using DTO.General.Surf.Input;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Customer.Database;
using System.Collections.Generic;

namespace DTO.Integration.Surf.Subscription.Input
{
    public class SurfSubscriptionInput
    {
        public SurfSubscriptionInput(string customerCode, SurfSubscriptionsInput subscriptions)
        {
            CustomerCode = customerCode;
            Subscriptions = subscriptions != null ? new List<SurfSubscriptionsInput> { subscriptions } : null;
        }

        public string CustomerCode { get; set; }
        public IEnumerable<SurfSubscriptionsInput> Subscriptions { get; set; }

    }

    public class SurfSubscriptionsInput
    {
        public SurfSubscriptionsInput(HubCustomer customer, SurfDeaflympicsManagamentInput management)
        {
            if (customer != null) {
                Name = customer?.Name;
                Document = customer?.Document?.Data;
            }

            if (management == null)
                return;

            PlanId = management.SurfPlanId;
            Iccid = management.Iccid;
            _ = int.TryParse(management.DDD, out var ddd);
            Ddd = ddd;
        }

        public SurfSubscriptionsInput(HubCustomer customer, HubCellphoneManagement management)
        {
            if (customer != null)
            {
                Name = customer?.Name;
                Document = customer?.Document?.Data;
            }

            if (management == null)
                return;

            PlanId = management.SurfPlanId;
            Iccid = management.ChipSerial;
            _ = int.TryParse(management.CellphoneData?.DDD, out var ddd);
            Ddd = ddd;
        }

        public string PlanId { get; set; }
        public string Iccid { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public int Ddd { get; set; }

    }
}
