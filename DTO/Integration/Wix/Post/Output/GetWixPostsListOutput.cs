using DTO.Integration.Wix.Base;
using System;
using System.Collections.Generic;

namespace DTO.Integration.Wix.Post.Output
{
    public class GetWixPostsListOutput
    {
        public List<WixPostDataOutput> Posts { get; set; }
        public WixMetaData MetaData { get; set; }
    }
}
