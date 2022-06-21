using DAO.DBConnection;
using DAO.Intra.Game;
using DAO.Intra.UserDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Game.Database;
using System.Linq;
using Useful.Extensions;

namespace Business.API.Intra.Game
{
    public class BlGame
    {
        private readonly IntraUserDAO IntraUserDAO;
        private readonly IntraGameDAO IntraGameDAO;

        public BlGame(XDataDatabaseSettings settings)
        {
            IntraUserDAO = new(settings);
            IntraGameDAO = new(settings);
        }

        public BaseApiOutput FinishGame(IntraGame game)
        {
            if (IntraUserDAO.FindById(game.PlayerOneId) == null)
                return new("Jogador Um não encontrado!");

            if (IntraUserDAO.FindById(game.PlayerTwoId) == null)
                return new("Jogador Dois não encontrado!");

            if (!(game?.Sets?.Any() ?? false))
                return new("Sets não informado!");

            foreach (var set in game.Sets)
            {
                if (set.PlayerOnePoints > 30)
                    return new($"O jogador Um passou o limite de pontos no Set {set.SetNumber}!");

                if (set.PlayerTwoPoints > 30)
                    return new($"O jogador Dois passou o limite de pontos no Set {set.SetNumber}!");

                if (set.PlayerOnePoints < 21 && set.PlayerTwoPoints < 21)
                    return new($"Set {set.SetNumber}, não foi finalizado!");

                if (NumberExtension.GetDiff(set.PlayerOnePoints, set.PlayerTwoPoints) != 2)
                    return new($"A diferença de Pontos está diferente de 2 no Set {set.SetNumber}!");
            }

            IntraGameDAO.Insert(game);
            return new(true);
        }
    }
}
