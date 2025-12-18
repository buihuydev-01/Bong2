using System.Collections.Generic;

namespace BettingOddsDisplay.Models
{
    public class BettingData
    {
        public List<League> Leagues { get; set; } = new List<League>();
        public List<Match> Matches { get; set; } = new List<Match>();
        public string UpdateTime { get; set; } = string.Empty;
    }
}
