using DTO.General.Base.Database;
using System;

namespace DTO.Integration.Surf.Token.Database
{
    public class SurfTokenInfo : BaseData
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
    }
}
