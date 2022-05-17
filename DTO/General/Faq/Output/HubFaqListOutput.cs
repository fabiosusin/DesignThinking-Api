using DTO.General.Base.Api.Output;
using DTO.Hub.Application.Faq.Database;
using System.Collections.Generic;

namespace DTO.General.Faq.Output
{
    public class HubFaqListOutput : BaseApiOutput
    {
        public HubFaqListOutput(string msg) : base(msg) { }
        public HubFaqListOutput(IEnumerable<AllyFaq> faqs) : base(true) => Faqs = faqs;
        public IEnumerable<AllyFaq> Faqs { get; set; }
    }
}
