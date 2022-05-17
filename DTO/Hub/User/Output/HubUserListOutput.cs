using DTO.General.Base.Api.Output;
using DTO.Hub.User.Database;
using System.Collections.Generic;

namespace DTO.Hub.User.Output
{
    public class HubUserListOutput : BaseApiOutput
    {
        public HubUserListOutput(string msg) : base(msg) { }
        public HubUserListOutput(IEnumerable<HubUser> allys) : base(true) => Users = allys;
        public IEnumerable<HubUser> Users { get; set; }
    }
}
