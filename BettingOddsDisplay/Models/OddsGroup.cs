using System.Collections.Generic;

namespace BettingOddsDisplay.Models
{
    public class OddsGroup
    {
        public int BetType { get; set; }
        public List<OddsLine> Lines { get; set; } = new List<OddsLine>();
    }

    public class OddsLine
    {
        public long OddsId { get; set; }
        public double Handicap { get; set; }
        public double HomeOdds { get; set; }
        public double AwayOdds { get; set; }
        public double DrawOdds { get; set; }
        public int Priority { get; set; }
        
        // For display formatting
        public string HandicapDisplay
        {
            get
            {
                if (Handicap == 0) return "0-0";
                if (Handicap > 0) return $"{Handicap:0.0#}";
                return $"{Handicap:0.0#}";
            }
        }

        public string HomeOddsDisplay => FormatOdds(HomeOdds);
        public string AwayOddsDisplay => FormatOdds(AwayOdds);
        public string DrawOddsDisplay => DrawOdds > 0 ? $"{DrawOdds:0.00}" : "";

        private string FormatOdds(double odds)
        {
            if (odds == 0) return "";
            if (odds > 0) return $"{odds:0.00}";
            return $"{odds:0.00}";
        }

        public string HomeOddsColor => HomeOdds > 0 ? "Black" : "Red";
        public string AwayOddsColor => AwayOdds > 0 ? "Black" : "Red";
    }
}
