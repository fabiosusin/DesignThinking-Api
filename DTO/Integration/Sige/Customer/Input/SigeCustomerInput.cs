using DTO.Hub.Ally.Database;
using System;

namespace DTO.Integration.Sige.Customer.Input
{
    public class SigeCustomerInput
    {
        public SigeCustomerInput(HubAlly input)
        {
            if (input == null)
                return;

            ID = input.SigeId;
            PessoaFisica = false;
            NomeFantasia = input.Name;
            RazaoSocial = input.CorporateName;
            CNPJ_CPF = input.Cnpj;
            IE = input.IE;
            Email = input.Email;
            Cliente = true;

            if (input.Address.IsValid())
            {
                Logradouro = input.Address.Street;
                LogradouroNumero = input.Address.Number;
                Complemento = input.Address.Complement;
                Bairro = input.Address.Neighborhood;
                Cidade = input.Address.City;
                Pais = input.Address.Country;
                CEP = input.Address.ZipCode;
                UF = input.Address.State;
                EnderecoPadrao = new(input.Address);
            }

        }

        public string ID { get; set; }
        public DateTime UltimaAlteracao { get; set; }
        public bool PessoaFisica { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ_CPF { get; set; }
        public string RG { get; set; }
        public string IE { get; set; }
        public string Logradouro { get; set; }
        public string LogradouroNumero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string CodigoMunicipio { get; set; }
        public string Pais { get; set; }
        public string CodigoPais { get; set; }
        public string CEP { get; set; }
        public string UF { get; set; }
        public string CodigoUF { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public bool Cliente { get; set; }
        public bool Tecnico { get; set; }
        public bool Vendedor { get; set; }
        public bool Transportadora { get; set; }
        public bool Fonecedor { get; set; }
        public bool Representada { get; set; }
        public string Ramo { get; set; }
        public string VendedorPadrao { get; set; }
        public string EmailLoginEcommerce { get; set; }
        public string Senha { get; set; }
        public string Salt { get; set; }
        public bool Bloqueado { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string Naturalidade { get; set; }
        public int ValorMinimoCompra { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool EstaInadimplente { get; set; }
        public SigeDefaultAddress EnderecoPadrao { get; set; }
    }
}
