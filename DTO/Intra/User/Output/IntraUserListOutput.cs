using DTO.General.Base.Api.Output;
using DTO.Intra.User.Database;
using System.Collections.Generic;

namespace DTO.Intra.User.Output
{
    public class IntraUserListOutput : BaseApiOutput
    {
        public IntraUserListOutput(string msg) : base(msg) { }
        public IntraUserListOutput(IEnumerable<IntraUser> allys) : base(true) => Users = allys;
        public IEnumerable<IntraUser> Users { get; set; }
    }
}
