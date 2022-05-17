using System;
using System.Collections.Generic;

namespace DTO.Integration.Sige.Order.Output
{
    public class SigeOrderOutput
    {

        public object Id { get; set; }
        public object AndroidVendaId { get; set; }
        public long Codigo { get; set; }
        public object OrigemVenda { get; set; }
        public string DepositoId { get; set; }
        public object Tabela { get; set; }
        public object TabelaId { get; set; }
        public string Deposito { get; set; }
        public string StatusSistema { get; set; }
        public object Status { get; set; }
        public object Categoria { get; set; }
        public DateTime Validade { get; set; }
        public string Empresa { get; set; }
        public object EmpresaId { get; set; }
        public object ClienteId { get; set; }
        public string Cliente { get; set; }
        public object PessoaId { get; set; }
        public string ClienteCnpj { get; set; }
        public object ClienteEmail { get; set; }
        public object Vendedor { get; set; }
        public string PlanoDeConta { get; set; }
        public string FormaPagamento { get; set; }
        public object FormaPagamentoId { get; set; }
        public long NumeroParcelas { get; set; }
        public long FreteMeioEnvio { get; set; }
        public object Transportadora { get; set; }
        public object FreteFormaEnvio { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime PrevisaoEntrega { get; set; }
        public DateTime DataPostagem { get; set; }
        public bool Enviado { get; set; }
        public long ValorFrete { get; set; }
        public bool FreteContaEmitente { get; set; }
        public object CodigoRastreio { get; set; }
        public bool EnderecoOpcional { get; set; }
        public long ValorSeguro { get; set; }
        public object Descricao { get; set; }
        public long OutrasDespesas { get; set; }
        public object TransacaoCartao { get; set; }
        public double ValorFinal { get; set; }
        public bool Finalizado { get; set; }
        public bool Lancado { get; set; }
        public object Municipio { get; set; }
        public object CodigoMunicipio { get; set; }
        public object Pais { get; set; }
        public object Cep { get; set; }
        public object Uf { get; set; }
        public object UfCodigo { get; set; }
        public object Bairro { get; set; }
        public object Logradouro { get; set; }
        public object LogradouroNumero { get; set; }
        public object LogradouroComplemento { get; set; }
        public List<SigeProductOrderOutput> Items { get; set; }
        public object Banco { get; set; }
        public DateTime Data { get; set; }
        public object Pagamentos { get; set; }
        public bool LancarComissaoVendedor { get; set; }
        public long ValorComissaoVendedor { get; set; }
        public object NumeroNFe { get; set; }
        public DateTime DataFaturamento { get; set; }
        public object ChaveAcessoNFe { get; set; }
        public object DanfeUrl { get; set; }
        public object UrlSefaz { get; set; }
        public object OrdemServico { get; set; }
        public object CodigoPedidoCliente { get; set; }
        public DateTime DataAprovacaoPedido { get; set; }
    }
}
