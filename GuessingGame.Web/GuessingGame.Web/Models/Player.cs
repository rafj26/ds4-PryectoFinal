using System;

namespace GuessingGame.Web.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int GuessCount { get; set; }
        public float TimeTaken { get; set; }
        public DateTime GameDate { get; set; }
    }
}