using DTO.General.Base.Database;
using System;

namespace DTO.Integration.SendPulse.Database
{
    public class SendPulseTokenInfo : BaseData
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
    }
}
