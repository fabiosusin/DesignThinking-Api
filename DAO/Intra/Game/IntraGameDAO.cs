using DAO.Base;
using DAO.DBConnection;
using DAO.Intra.Employee;
using DAO.Intra.UserDAO;
using DTO.General.DAO.Output;
using DTO.Intra.Employee.Database;
using DTO.Intra.Game.Database;
using DTO.Intra.Game.Input;
using DTO.Intra.Game.Output;
using DTO.Intra.User.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Intra.Game
{
    public class IntraGameDAO : IBaseDAO<IntraGame>
    {
        internal RepositoryMongo<IntraGame> Repository;
        private readonly IntraUserDAO IntraUserDAO;
        private readonly IntraEmployeeDAO IntraEmployeeDAO;
        public IntraGameDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
            IntraUserDAO = new(settings);
            IntraEmployeeDAO = new(settings);
        }

        public DAOActionResultOutput Insert(IntraGame obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(IntraGame obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(IntraGame obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(IntraGame obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public IntraGame FindOne() => Repository.FindOne();

        public IntraGame FindOne(Expression<Func<IntraGame, bool>> predicate) => Repository.FindOne(predicate);

        public IntraGame FindById(string id) => Repository.FindById(id);

        public IEnumerable<IntraGame> Find(Expression<Func<IntraGame, bool>> predicate) => Repository.Collection.Find(Query<IntraGame>.Where(predicate));

        public IEnumerable<IntraGame> FindAll() => Repository.FindAll();

        public List<IntraGameReportData> Report(IntraGameListInput input)
        {
            IEnumerable<IntraGame> games = null;
            var users = IntraUserDAO.FindAll();
            var players = IntraEmployeeDAO.FindAll();
            if (input == null)
                games = FindAll();
            else if (input.Paginator == null)
                games = Repository.Collection.Find(GenerateFilters(input.Filters, users, players));
            else
                games = Repository.Collection.Find(GenerateFilters(input.Filters, users, players)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            var result = new List<IntraGameReportData>();
            foreach (var game in games ?? new List<IntraGame>())
            {
                var userName = users.FirstOrDefault(x => x.Id == game.UserId)?.Name;
                var playerOne = players.FirstOrDefault(x => x.Id == game.PlayerOneId);
                UpdateReportData(game, result, playerOne?.Id, playerOne?.Name, userName);

                var playerTwo = players.FirstOrDefault(x => x.Id == game.PlayerTwoId);
                UpdateReportData(game, result, playerTwo?.Id, playerTwo?.Name, userName);
            }

            return result;
        }

        private static void UpdateReportData(IntraGame game, List<IntraGameReportData> result, string playerId, string playerName, string username)
        {
            if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(playerId))
                return;

            var player = result.FirstOrDefault(x => x.PlayerId == playerId);
            if (player == null)
            {
                player = new(playerId, playerName, username);
                result.Add(player);
            }

            player.TotalGames++;
            var setsWins = 0;
            var isPlayerOne = game.PlayerOneId == playerId;

            foreach (var set in game.Sets)
            {
                if (isPlayerOne)
                {
                    if (set.PlayerOne.Points > set.PlayerTwo.Points)
                        setsWins++;

                    var hits =
                        set.PlayerOne.ShotChart.First +
                        set.PlayerOne.ShotChart.Second +
                        set.PlayerOne.ShotChart.Third +
                        set.PlayerOne.ShotChart.Fourth +
                        set.PlayerOne.ShotChart.Fifth +
                        set.PlayerOne.ShotChart.Sixth;

                    player.TotalHits += hits;
                    player.TotalErrors += set.PlayerOne.ShotChart.Errors;
                    player.TotalShots += hits + set.PlayerOne.ShotChart.Errors;
                }
                else
                {
                    if (set.PlayerOne.Points < set.PlayerTwo.Points)
                        setsWins++;


                    var hits =
                        set.PlayerTwo.ShotChart.First +
                        set.PlayerTwo.ShotChart.Second +
                        set.PlayerTwo.ShotChart.Third +
                        set.PlayerTwo.ShotChart.Fourth +
                        set.PlayerTwo.ShotChart.Fifth +
                        set.PlayerTwo.ShotChart.Sixth;

                    player.TotalHits += hits;
                    player.TotalErrors += set.PlayerTwo.ShotChart.Errors;
                    player.TotalShots += hits + set.PlayerTwo.ShotChart.Errors;
                }
            }

            player.HitsPercentual = player.TotalShots == 0 ? 0 : (player.TotalHits * 100) / player.TotalShots;
            if (setsWins == 3)
                player.TotalWins++;
            else
                player.TotalLoses++;


        }

        public List<IntraGameOutput> List(IntraGameListInput input)
        {
            IEnumerable<IntraGame> games = null;
            var users = IntraUserDAO.FindAll();
            var players = IntraEmployeeDAO.FindAll();
            if (input == null)
                games = FindAll();
            else if (input.Paginator == null)
                games = Repository.Collection.Find(GenerateFilters(input.Filters, users, players));
            else
                games = Repository.Collection.Find(GenerateFilters(input.Filters, users, players)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            var result = new List<IntraGameOutput>();
            foreach (var game in games ?? new List<IntraGame>())
            {
                result.Add(new IntraGameOutput
                {
                    Username = users.FirstOrDefault(x => x.Id == game.UserId)?.Name,
                    PlayerOneName = players.FirstOrDefault(x => x.Id == game.PlayerOneId)?.Name,
                    PlayerTwoName = players.FirstOrDefault(x => x.Id == game.PlayerTwoId)?.Name,
                    Scoreboard = GetScoreboard(game)
                });
            }

            return result;
        }

        private static string GetScoreboard(IntraGame game)
        {
            if (!(game?.Sets?.Any() ?? false))
                return null;

            var setsPlayerOne = 0;
            var setsPlayerTwo = 0;
            foreach (var set in game.Sets)
            {
                if (set.PlayerOne.Points > set.PlayerTwo.Points)
                    setsPlayerOne++;
                else
                    setsPlayerTwo++;
            }

            return $"{setsPlayerOne} X {setsPlayerTwo}";
        }


        private static IMongoQuery GenerateFilters(IntraGameFiltersInput input, IEnumerable<IntraUser> users, IEnumerable<IntraEmployee> players)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.UserName) && (users?.Any() ?? false))
            {
                var usersId = users.Where(x => x.Name.ToLower().Contains(input.UserName.ToLower()))?.Select(x => x.Id);
                queryList.Add(Query<IntraGame>.In(x => x.UserId, usersId));
            }

            if (!string.IsNullOrEmpty(input.PlayerName) && (users?.Any() ?? false))
            {
                var playersId = players.Where(x => x.Name.ToLower().Contains(input.PlayerName.ToLower()))?.Select(x => x.Id);
                queryList.Add(Query.Or(Query<IntraGame>.In(x => x.PlayerOneId, playersId), Query<IntraGame>.In(x => x.PlayerTwoId, playersId)));
            }

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
