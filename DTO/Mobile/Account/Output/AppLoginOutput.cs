using DTO.General.Base.Api.Output;
using System;

namespace DTO.Mobile.Account.Output
{
    public class AppLoginOutput : BaseApiOutput
    {
        public AppLoginOutput(string msg) : base(msg) { }

        public AppLoginOutput(string id, string name, string cellphone, string cellphonePrefix) : base(true)
        {
            Id = id;
            Name = name;
            Cellphone = cellphone;
            CellphoneCountryPrefix = cellphonePrefix;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public string CellphoneCountryPrefix { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
