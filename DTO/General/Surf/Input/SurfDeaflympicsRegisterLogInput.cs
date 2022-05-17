namespace DTO.General.Surf.Input
{

    public class SurfDeaflympicsRegisterLogInput
    {
        public SurfDeaflympicsRegisterLogInput(string iccid, string message, string method, object data)
        {
            IccId = iccid;
            Message = message;
            Method = method;
            Data = data;
        }

        public SurfDeaflympicsRegisterLogInput(string iccid, string message, string method, string exception, object data)
        {
            IccId = iccid;
            Message = message;
            Method = method;
            ExceptionMessage = exception;
            Data = data;
        }

        public string Message { get; set; }
        public string IccId { get; set; }
        public string Method { get; set; }
        public string ExceptionMessage { get; set; }
        public object Data { get; set; }
    }
}
