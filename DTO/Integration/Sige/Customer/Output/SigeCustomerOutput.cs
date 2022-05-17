using DTO.Integration.Sige.Customer.Input;
using DTO.Hub.Ally.Database;
using System;

namespace DTO.Integration.Sige.Customer.Output
{
    public class SigeCustomerOutput
    {

        public string ID { get; set; }
        public DateTime UltimaAlteracao { get; set; }
        public bool PessoaFisica { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ_CPF { get; set; }
        public object RG { get; set; }
        public object IE { get; set; }
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
        public object Telefone { get; set; }
        public object Celular { get; set; }
        public object Email { get; set; }
        public object Site { get; set; }
        public bool Cliente { get; set; }
        public bool Tecnico { get; set; }
        public bool Vendedor { get; set; }
        public bool Transportadora { get; set; }
        public bool Fonecedor { get; set; }
        public bool Representada { get; set; }
        public object Ramo { get; set; }
        public object VendedorPadrao { get; set; }
        public object EmailLoginEcommerce { get; set; }
        public object Senha { get; set; }
        public object Salt { get; set; }
        public bool Bloqueado { get; set; }
        public object NomePai { get; set; }
        public object NomeMae { get; set; }
        public object Naturalidade { get; set; }
        public decimal ValorMinimoCompra { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool EstaInadimplente { get; set; }
        public SigeDefaultAddress EnderecoPadrao { get; set; }
    }
}
