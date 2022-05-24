using DAO.DBConnection;
using DTO.Intra.Menu.Enum;
using DTO.Intra.Menu.Output;
using System.Collections.Generic;

namespace Business.API.Intra.Menu
{
    public class BlIntraMenu
    {
        public BlIntraMenu(XDataDatabaseSettings settings)
        {
        }

        public static List<IntraMenuOutput> GetIntraMenu()
        {
            var menus = new List<IntraMenuOutput>();
            var registers = new IntraMenuOutput("Cadastros", new("fas fa-edit", IntraIconTypeEnum.FontAwesome), new List<IntraMenuOutput>
            {
                new("Usuários", new("fas fa-user-cog", IntraIconTypeEnum.FontAwesome), "users"),
                new("Jogadores", new("fas fa-users", IntraIconTypeEnum.FontAwesome), "employee")
            });

            menus.Add(new("Dashboard", new("dashboard", IntraIconTypeEnum.Material), "home"));
            menus.Add(registers);

            return menus;
        }
    }
}
