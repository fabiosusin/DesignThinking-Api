using DAO.DBConnection;
using DAO.Hub.Application.Faq;
using DTO.General.Faq.Input;
using DTO.General.Faq.Output;
using System.Linq;

namespace Business.API.General.Faq
{
    public class BlFaq
    {
        private readonly AllyFaqDAO FaqDAO;

        public BlFaq(XDataDatabaseSettings settings) => FaqDAO = new(settings);

        public HubFaqListOutput ListFaq(FaqListInput input)
        {
            var result = FaqDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma pergunta encontrada!");

            return new(result);
        }

    }
}
