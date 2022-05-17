using System;
using System.Collections.Generic;

namespace DTO.Integration.Sige.Entry.Input
{
    public class SigeEntryInput
    {

        public SigeEntryInput(decimal price, string documentNumber, string companyName, string customerName, string accountPlanName)
        {
            DataVencimentoOriginal = DataVencimento = DataCompetencia = DateTime.Now;
            Empresa = companyName;
            Cliente = customerName;
            EhDespesa = true;
            PlanoDeConta = accountPlanName;
            NumeroDocumento = documentNumber;
            Valor = price;
        }

        public DateTime DataVencimentoOriginal { get; set; }
        public int Codigo { get; set; }
        public DateTime DataCompetencia { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Empresa { get; set; }
        public string Cliente { get; set; }
        public string NumeroDocumento { get; set; }
        public string Descricao { get; set; }
        public string Observacoes { get; set; }
        public bool Quitado { get; set; }
        public DateTime DataQuitacao { get; set; }
        public bool Conciliado { get; set; }
        public bool EhDespesa { get; set; }
        public string PlanoDeConta { get; set; }
        public string CentroDeCusto { get; set; }
        public string ContaBancaria { get; set; }
        public string FormaPagamento { get; set; }
        public string LancamentoGrupo { get; set; }
        public decimal Valor { get; set; }
        public int TotalRecebido { get; set; }
        public List<SigeEntryPayment> Pagamentos { get; set; }
        public string ModoParcelamento { get; set; }
        public string Intervalo { get; set; }
        public int DiasIntervalo { get; set; }
        public int Juro { get; set; }
        public bool JurosCompostos { get; set; }
        public int NumeroParcelas { get; set; }
        public List<SigeEntryParcell> Parcelas { get; set; }
    }

    public class SigeEntryPayment
    {
        public DateTime Data { get; set; }
        public string FormaPagamento { get; set; }
        public string NumeroDocumento { get; set; }
        public string ContaBancaria { get; set; }
        public bool Conciliado { get; set; }
        public int Valor { get; set; }
    }

    public class SigeEntryParcell
    {
        public DateTime DataVencimento { get; set; }
        public string Documento { get; set; }
        public int ValorParcela { get; set; }
        public bool Quitado { get; set; }
        public DateTime DataQuitacao { get; set; }
        public int TotalRecebido { get; set; }
    }
}
