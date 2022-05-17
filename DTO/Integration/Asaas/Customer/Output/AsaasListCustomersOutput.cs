using System.Collections.Generic;

namespace DTO.Integration.Asaas.Customer.Output
{
    public class AsaasListCustomersOutput
    {
        public bool HasMore { get; set; }
        public int TotalCount { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public List<AsaasCreateCustomerOutput> Data { get; set; }
    }
}
