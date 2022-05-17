using DTO.General.Base.Database;
using System;

namespace DTO.Mobile.Account.Database
{
    public class AppOtpCode : BaseData
    {
        public AppOtpCode() { }

        public AppOtpCode(string appMobileId, string code, DateTime expiration)
        {
            AppMobileId = appMobileId;
            Code = code;
            Expiration = expiration;
        }

        public string AppMobileId { get; set; }
        public string Code { get; set; }
        public bool Used { get; set; }
        public DateTime Expiration { get; set; }
    }
}
