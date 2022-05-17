using DTO.General.Base.Database;
using DTO.Hub.Application.Faq.Input;

namespace DTO.Hub.Application.Faq.Database
{
    public class AllyFaq : BaseData
    {
        public AllyFaq () { }
        public AllyFaq(HubAppFaqInput input)
        {
            if (input == null)
                return;

            AllyId = input.AllyId;
            Answer = input.Answer;
            Question = input.Question;
            Linked = input.Linked;
        }

        public AllyFaq(HubAppFaqInput input, string id)
        {
            if (input == null)
                return;

            Id = id;
            AllyId = input.AllyId;
            Answer = input.Answer;
            Question = input.Question;
            Linked=input.Linked;
        }

        public string AllyId { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
        public bool Linked { get; set; }
    }
}
