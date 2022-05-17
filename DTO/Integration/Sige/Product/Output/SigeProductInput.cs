using System;
using System.Collections.Generic;

namespace DTO.Integration.Sige.Product.Output
{
    public class SigeProductInput
    {
        public string Id { get; set; }
        public DateTime UltimaAlteracao { get; set; }
        public string Categoria { get; set; }
        public string Fornecedor { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Genero { get; set; }
        public string Especificacao { get; set; }
        public long PesoKg { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal LucroDinheiro { get; set; }
        public long LucroPercentual { get; set; }
        public decimal PrecoVenda { get; set; }
        public long PrecoMinimoVenda { get; set; }
        public long EstoqueRisco { get; set; }
        public long DepositoPadrao { get; set; }
        public long EstoqueSaldo { get; set; }
        public string Ncm { get; set; }
        public string UnidadeTributavel { get; set; }
        public long OrigemMercadoria { get; set; }
        public string Cest { get; set; }
        public bool ProduzidoEscalaNaoRelevante { get; set; }
        public string IpiSituacaoTributaria { get; set; }
        public long Ipi { get; set; }
        public bool ProdutoPossuiUcomDiferenteDeUtrib { get; set; }
        public long FatorDeConversaoUnidadeComercialParaTributavel { get; set; }
        public long EanUnidadeTributavelNFe { get; set; }
        public string UnidadeTributavelNFe { get; set; }
        public bool PossuiRastreabilidadeSefaz { get; set; }
        public bool ProdutoPossuiInfComb { get; set; }
        public long PercentualGlp { get; set; }
        public long PercentualGlpNacional { get; set; }
        public long PercentualGlpImportado { get; set; }
        public long ValorPartidaGlp { get; set; }
        public bool EhArmadeFogo { get; set; }
        public long ArmaTipo { get; set; }
        public object ArmaDescricao { get; set; }
        public bool PossuiInformacoesStRetidosAnteriormente { get; set; }
        public long AliquotaSuportadaConsumidorFinal { get; set; }
        public long FcpstBaseCalculo { get; set; }
        public long FcpstPercentual { get; set; }
        public long FcpstValor { get; set; }
        public long ValorIcmsSubstituto { get; set; }
        public long ValorIcmsSt { get; set; }
        public bool VisivelSite { get; set; }
        public bool VisivelVendas { get; set; }
        public bool DestaqueSite { get; set; }
        public bool IgnorarEstoque { get; set; }
        public bool FreteGratis { get; set; }
        public string FiltrosCategoria { get; set; }
        public long PercentualDescontoBoleto { get; set; }
        public long Comprimento { get; set; }
        public long Altura { get; set; }
        public long Largura { get; set; }
        public long Diametro { get; set; }
        public long TipoDoProduto { get; set; }
        public List<SigePriceTableInput> PrecosTabelas { get; set; }
        public List<string> Categorias { get; set; }
    }
}
