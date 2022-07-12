using DAO.DBConnection;
using DAO.Intra.Employee;
using DAO.Intra.Game;
using DTO.General.Base.Api.Output;
using DTO.Intra.Game.Database;
using DTO.Intra.Game.Input;
using DTO.Intra.Game.Output;
using System.Linq;
using Useful.Extensions;

namespace Business.API.Intra.Game
{
    public class BlGame
    {
        private readonly IntraGameDAO IntraGameDAO;
        private readonly IntraEmployeeDAO IntraEmployeeDAO;

        public BlGame(XDataDatabaseSettings settings)
        {
            IntraEmployeeDAO = new(settings);
            IntraGameDAO = new(settings);
        }

        public BaseApiOutput FinishGame(IntraGame game)
        {
            if (IntraEmployeeDAO.FindById(game.PlayerOneId) == null)
                return new("Jogador Um não encontrado!");

            if (IntraEmployeeDAO.FindById(game.PlayerTwoId) == null)
                return new("Jogador Dois não encontrado!");

            if (!(game?.Sets?.Any() ?? false))
                return new("Sets não informado!");

            foreach (var set in game.Sets)
            {
                if (set.PlayerOne?.Points == 30 || set.PlayerTwo?.Points == 30)
                    continue;

                if (set.PlayerOne?.Points > 30)
                    return new($"O jogador Um passou o limite de pontos no Set {set.SetNumber}!");

                if (set.PlayerTwo?.Points > 30)
                    return new($"O jogador Dois passou o limite de pontos no Set {set.SetNumber}!");

                if (set.PlayerOne?.Points < 21 && set.PlayerTwo?.Points < 21)
                    return new($"Set {set.SetNumber}, não foi finalizado!");

                if (NumberExtension.GetDiff(set.PlayerOne?.Points ?? 0, set.PlayerTwo?.Points ?? 0) < 2)
                    return new($"A diferença de Pontos está diferente de 2 no Set {set.SetNumber}!");
            }

            IntraGameDAO.Insert(game);
            return new(true);
        }

        public IntraGameListOutput List(IntraGameListInput input)
        {
            var result = IntraGameDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Jogo encontrado!");

            return new(result);
        }

        public IntraGameReportOutput Report(IntraGameListInput input)
        {
            var result = IntraGameDAO.Report(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Jogo encontrado!");

            return new(result);
        }

    }
}
