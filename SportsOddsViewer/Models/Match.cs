using System;
using System.ComponentModel;

namespace SportsOddsViewer.Models
{
    public class Match : INotifyPropertyChanged
    {
        private string _time = string.Empty;
        private string _status = string.Empty;
        private string _homeTeam = string.Empty;
        private string _awayTeam = string.Empty;
        private string _league = string.Empty;
        private string _score = string.Empty;
        private string _odds1X2Home = string.Empty;
        private string _odds1X2Draw = string.Empty;
        private string _odds1X2Away = string.Empty;
        private string _oddsOU = string.Empty;
        private string _oddsOUOver = string.Empty;
        private string _oddsOUUnder = string.Empty;
        private string _oddsHDPHome = string.Empty;
        private string _oddsHDPLine = string.Empty;
        private string _oddsHDPAway = string.Empty;

        public int EventId { get; set; }
        public int LeagueId { get; set; }

        public string Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(nameof(Time)); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public string HomeTeam
        {
            get => _homeTeam;
            set { _homeTeam = value; OnPropertyChanged(nameof(HomeTeam)); }
        }

        public string AwayTeam
        {
            get => _awayTeam;
            set { _awayTeam = value; OnPropertyChanged(nameof(AwayTeam)); }
        }

        public string League
        {
            get => _league;
            set { _league = value; OnPropertyChanged(nameof(League)); }
        }

        public string Score
        {
            get => _score;
            set { _score = value; OnPropertyChanged(nameof(Score)); }
        }

        public string Odds1X2Home
        {
            get => _odds1X2Home;
            set { _odds1X2Home = value; OnPropertyChanged(nameof(Odds1X2Home)); }
        }

        public string Odds1X2Draw
        {
            get => _odds1X2Draw;
            set { _odds1X2Draw = value; OnPropertyChanged(nameof(Odds1X2Draw)); }
        }

        public string Odds1X2Away
        {
            get => _odds1X2Away;
            set { _odds1X2Away = value; OnPropertyChanged(nameof(Odds1X2Away)); }
        }

        public string OddsOU
        {
            get => _oddsOU;
            set { _oddsOU = value; OnPropertyChanged(nameof(OddsOU)); }
        }

        public string OddsOUOver
        {
            get => _oddsOUOver;
            set { _oddsOUOver = value; OnPropertyChanged(nameof(OddsOUOver)); }
        }

        public string OddsOUUnder
        {
            get => _oddsOUUnder;
            set { _oddsOUUnder = value; OnPropertyChanged(nameof(OddsOUUnder)); }
        }

        public string OddsHDPHome
        {
            get => _oddsHDPHome;
            set { _oddsHDPHome = value; OnPropertyChanged(nameof(OddsHDPHome)); }
        }

        public string OddsHDPLine
        {
            get => _oddsHDPLine;
            set { _oddsHDPLine = value; OnPropertyChanged(nameof(OddsHDPLine)); }
        }

        public string OddsHDPAway
        {
            get => _oddsHDPAway;
            set { _oddsHDPAway = value; OnPropertyChanged(nameof(OddsHDPAway)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
