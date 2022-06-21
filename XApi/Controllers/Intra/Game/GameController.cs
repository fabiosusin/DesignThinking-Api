﻿using Business.API.Intra.Game;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.Game.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.Game
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Jogo"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Game")]
    public class GameController : BaseController<GameController>
    {
        public GameController(ILogger<GameController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlGame Bl;

        [HttpPost, Route("finish-game")]
        public IActionResult FinishGame(IntraGame input) => Ok(Bl.FinishGame(input));
    }
}
