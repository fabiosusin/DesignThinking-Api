using DTO.General.Base.Database;
using System.Collections.Generic;

namespace DTO.Intra.Game.Database
{
    public class IntraGame : BaseData
    {
        public string UserId { get; set; }
        public string PlayerOneId { get; set; }
        public string PlayerTwoId { get; set; }
        public List<IntraGameSet> Sets { get; set; }
    }

    public class IntraGameSet
    {
        public int SetNumber { get; set; }
        public int PlayerOnePoints { get; set; }
        public int PlayerTwoPoints { get; set; }
    }
}
