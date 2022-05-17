using System;

namespace DTO.Integration.Sige.AccountPlan.Output
{
    public class SigeAccountPlanOutput
    {
        public string Id { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int CodigoNatureza { get; set; }
        public string TipoDeConta { get; set; }
        public string Nome { get; set; }
        public bool Despesa { get; set; }
        public string Hierarquia { get; set; }
        public string GrupoDRE { get; set; }
        public bool DesativarPlano { get; set; }
        public string CentroDeCusto { get; set; }
        public string GrupoLancamento { get; set; }
    }
}
