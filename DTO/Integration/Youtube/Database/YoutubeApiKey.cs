using DTO.General.Base.Database;
using System;

namespace DTO.Integration.Youtube.Database
{
    public class YoutubeApiKey : BaseData
    {
        public string Key { get; set; }
        public DateTime ExpiredIn { get; set; }
    }
}
