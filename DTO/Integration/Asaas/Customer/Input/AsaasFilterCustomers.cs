namespace DTO.Integration.Asaas.Customer.Input
{
    public class AsaasFilterCustomers
    {
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}
