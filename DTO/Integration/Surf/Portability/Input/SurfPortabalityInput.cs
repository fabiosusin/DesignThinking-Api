using DTO.Hub.Cellphone.Database;
using DTO.Hub.Customer.Database;

namespace DTO.Integration.Surf.Portability.Input
{
    public class SurfPortabalityInput
    {
        public SurfPortabalityInput(CellphoneManagementPortability portability, HubCustomer customer, string subscriptionId)
        {
            SubscriptionId = subscriptionId;
            Msisdn = portability?.Number;
            OperatorId = portability?.OperatorId;
            Document = customer?.Document?.Data;
            Name = customer?.Name;
        }

        public string SubscriptionId { get; set; }
        public string Msisdn { get; set; }
        public string OperatorId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
    }
}
