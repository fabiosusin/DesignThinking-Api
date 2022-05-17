namespace DTO.Integration.Facilita.SMS.Output
{
    public class FacilitaSendSmsResultOutput
    {
        public FacilitaSendSmsResultOutput(string result)
        {
            _ = int.TryParse(result.Split(';')[0], out var type);
            if (type > 0 && type < 7)
                ResultType = (FacilitaResultEnum)type;

            Success = ResultType == FacilitaResultEnum.Success;
            Message = Success ? "Mensagem enviada com sucesso!" : "Não foi possível enviar a mensagem!";
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public FacilitaResultEnum ResultType { get; set; }
    }

    public enum FacilitaResultEnum
    {
        Unknown,
        InvalidLogin,
        UserWithoutCredit,
        InvalidCellphone,
        InvalidMessage,
        SmsScheduled,
        Success
    }
}
