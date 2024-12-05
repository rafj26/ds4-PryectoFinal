using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuessingGame.Web.Models
{
    public class GameResult
    {
        public string PlayerName { get; set; }
        public int GuessCount { get; set; }
        public float TimeTaken { get; set; }
    }
}