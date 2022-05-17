using System;

namespace DTO.Integration.Sige.Customer.Input
{
    public class SigeCustomerFiltersInput
    {
        public SigeCustomerFiltersInput(string cpf_cnpj) => CpfCnpj = cpf_cnpj;
        public string NomeFantasia { get; set; }
        public string CpfCnpj { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public bool Cliente { get; set; }
        public bool Fornecedor { get; set; }
        public DateTime AlteradoApos { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
    }
}
