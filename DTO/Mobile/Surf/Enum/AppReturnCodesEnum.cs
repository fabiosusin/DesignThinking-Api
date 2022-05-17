using System.ComponentModel;

namespace DTO.Surf.Enum
{
    public enum AppReturnCodesEnum
    {
        [Description("Unknown")]
        Unknown,
        [Description("P00")]
        P00, // Sucesso
        [Description("P01")]
        P01, // MSISDN é obrigatório
        [Description("P03")]
        P03, // Nenhum registro foi encontrado na base
        [Description("P04")]
        P04, // Transação duplicada
        [Description("P100")]
        P100, // Erro enquanto processava a transação
        [Description("P532")]
        P532, // IMSI Inválido
        [Description("P533")]
        P533, // ICCID Inválido
        [Description("P1010")]
        P1010, // Assinante Inválido
        [Description("P1020")]
        P1020, // Account ID Inválido
        [Description("P1064")]
        P1064, // Pacote não encontrado
        [Description("P1226")]
        P1226, // O pacote solicitado está atualmente em espera
        [Description("P2000")]
        P2000 // Assinante não pertence a rede
    }
}