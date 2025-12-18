namespace BettingOddsDisplay.Models
{
    public class League
    {
        public int LeagueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubLeague { get; set; } = string.Empty;
        public string MatchInfo { get; set; } = string.Empty;
    }
}
