using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using GuessingGame.Web.Models;
using GuessingGame.Web.Services;

namespace GuessingGame.Web.Controllers
{
    public class GameController : Controller
    {
        private const string TargetNumberKey = "TargetNumber";
        private const string PlayerNameKey = "PlayerName";
        private const string GameStartKey = "GameStart";
        private const string GuessCountKey = "GuessCount";
        private readonly ApiService _apiService;

        public GameController()
        {
            _apiService = new ApiService();
        }

        public ActionResult Index()
        {
            Session.Clear();
            return View(new GameViewModel());
        }

        [HttpPost]
        public ActionResult StartGame(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
                return RedirectToAction("Index");

            Random rnd = new Random();
            Session[TargetNumberKey] = rnd.Next(1, 101);
            Session[PlayerNameKey] = playerName;
            Session[GameStartKey] = DateTime.Now;
            Session[GuessCountKey] = 0;

            return RedirectToAction("Play");
        }

        public ActionResult Play()
        {
            if (Session[PlayerNameKey] == null)
                return RedirectToAction("Index");

            var model = new GameViewModel
            {
                PlayerName = Session[PlayerNameKey].ToString(),
                GameStart = (DateTime)Session[GameStartKey]
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> MakeGuess(int guess)
        {
            System.Diagnostics.Debug.WriteLine($"Número objetivo: {Session[TargetNumberKey]}");
            System.Diagnostics.Debug.WriteLine($"Intento actual: {guess}");

            int target = (int)Session[TargetNumberKey];
            int guessCount = (int)Session[GuessCountKey] + 1;
            Session[GuessCountKey] = guessCount;

            var model = new GameViewModel
            {
                PlayerName = Session[PlayerNameKey].ToString(),
                Guess = guess,
                GuessCount = guessCount,
                GameStart = (DateTime)Session[GameStartKey]
            };

            if (guess < target)
            {
                model.Message = "¡El número es mayor!";
                System.Diagnostics.Debug.WriteLine("El número es mayor");
            }
            else if (guess > target)
            {
                model.Message = "¡El número es menor!";
                System.Diagnostics.Debug.WriteLine("El número es menor");
            }
            else
            {
                model.Message = "¡Felicitaciones! ¡Has ganado!";
                model.GameWon = true;
                System.Diagnostics.Debug.WriteLine("¡Ha ganado!");

                float timeTaken = (float)(DateTime.Now - (DateTime)Session[GameStartKey]).TotalSeconds;

                try
                {
                    await _apiService.SaveGameResult(new GameResult
                    {
                        PlayerName = model.PlayerName,
                        GuessCount = guessCount,
                        TimeTaken = timeTaken
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al guardar resultado: {ex.Message}");
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetLeaderboard()
        {
            var players = await _apiService.GetTopPlayers();
            return PartialView("_Leaderboard", players);
        }
    }
}