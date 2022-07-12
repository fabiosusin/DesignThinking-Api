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
        public IntraPlayerGame PlayerOne { get; set; }
       public IntraPlayerGame PlayerTwo { get; set; }
    }

    public class IntraPlayerGame
    {
        public int Points { get; set; }
        public IntraGameSetShotChart ShotChart { get; set; }
    }

    public class IntraGameSetShotChart
    {
        public int First { get; set; }
        public int Second { get; set; }
        public int Third { get; set; }
        public int Fourth { get; set; }
        public int Fifth { get; set; }
        public int Sixth { get; set; }
        public int Errors { get; set; }
    }
}