using System;
using System.Collections.Generic;

namespace BettingOddsDisplay.Models
{
    public class Match
    {
        public long MatchId { get; set; }
        public int SportType { get; set; }
        public int LeagueId { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public string MatchCode { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime MatchTime { get; set; }
        public int LiveStatus { get; set; }
        public string StreamType { get; set; } = string.Empty;
        public int Flags { get; set; }
        
        public long MarketId { get; set; }
        public List<OddsGroup> OddsGroups { get; set; } = new List<OddsGroup>();
        
        // For display
        public string TimeDisplay => MatchTime.ToString("HH:mm");
        public string StatusDisplay => Status == 8 ? "Live" : "";
    }
}
