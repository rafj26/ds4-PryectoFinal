using GuessingGame.API.Models;
using GuessingGame.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuessingGame.API.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private readonly GameService _gameService;

        public GameController()
        {
            _gameService = new GameService();
        }

        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveGameResult([FromBody] GameResult result)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _gameService.SaveGameResult(result);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("top")]
        public async Task<IHttpActionResult> GetTopPlayers()
        {
            try
            {
                var players = await _gameService.GetTopPlayers();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}