using DTO.General.Base.Api.Output;
using DTO.Hub.User.Database;
using System;

namespace DTO.Hub.User.Output
{
    public class HubLoginOutput : BaseApiOutput
    {
        public HubLoginOutput(string msg) : base(msg) { }

        public HubLoginOutput(HubUser input, bool isMasterAlly) : base(true)
        {
            if (input == null)
                return;

            Id = input.Id;  
            Name = input.Name;
            Email = input.Email;
            Password = input.Password;
            TempPassword = input.TempPassword;
            AllyId = input.AllyId;
            IsMasterAlly = isMasterAlly;
        }

        public bool IsMasterAlly { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string AllyId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TempPassword { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
