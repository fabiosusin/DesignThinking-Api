﻿using DTO.General.Base.Api.Output;
using DTO.Mobile.Account.Output;
using DTO.Hub.User.Output;
using System;
using DTO.Intra.User.Output;

namespace DTO.General.Login.Output
{
    public class LoginOutput : BaseApiOutput
    {
        public LoginOutput(string msg) : base(msg) { }

        public LoginOutput(AppLoginOutput input) : base(true)
        {
            if (input == null)
                return;

            Id = input.Id;
            AccessToken = input.AccessToken;
            Cellphone = input.Cellphone;
            AccessTokenExpiration = input.AccessTokenExpiration;
        }

        public LoginOutput(HubLoginOutput input) : base(true)
        {
            if (input == null)
                return;

            Id = input.Id;
            Email = input.Email;
            AccessToken = input.AccessToken;
            AccessTokenExpiration = input.AccessTokenExpiration;
        }

        public LoginOutput(IntraLoginOutput input) : base(true)
        {
            if (input == null)
                return;

            Id = input.Id;
            Email = input.Email;
            AccessToken = input.AccessToken;
            AccessTokenExpiration = input.AccessTokenExpiration;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Cellphone { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
