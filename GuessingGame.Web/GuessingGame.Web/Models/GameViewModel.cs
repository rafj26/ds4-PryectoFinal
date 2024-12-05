using System;

namespace GuessingGame.Web.Models
{
    public class GameViewModel
    {
        public string PlayerName { get; set; }
        public int? Guess { get; set; }
        public string Message { get; set; }
        public bool GameWon { get; set; }
        public DateTime GameStart { get; set; }
        public int GuessCount { get; set; }
    }
}