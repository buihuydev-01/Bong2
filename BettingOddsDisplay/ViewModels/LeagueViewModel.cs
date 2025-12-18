using System.Collections.Generic;
using System.Collections.ObjectModel;
using BettingOddsDisplay.Models;

namespace BettingOddsDisplay.ViewModels
{
    public class LeagueViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string SubLeague { get; set; } = string.Empty;
        public string MatchInfo { get; set; } = string.Empty;
        public ObservableCollection<Match> Matches { get; set; } = new ObservableCollection<Match>();
    }
}
