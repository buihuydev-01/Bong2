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
        
        // Full Time (Nguyên trận) - HDP
        private string _ftHDPLine = string.Empty;
        private string _ftHDPHome = string.Empty;
        private string _ftHDPAway = string.Empty;
        
        // Full Time - OU
        private string _ftOULine = string.Empty;
        private string _ftOUOver = string.Empty;
        private string _ftOUUnder = string.Empty;
        
        // Full Time - 1X2
        private string _ft1X2Home = string.Empty;
        private string _ft1X2Draw = string.Empty;
        private string _ft1X2Away = string.Empty;
        
        // Odd/Even (Lẻ/Chẵn)
        private string _oddEvenOdd = string.Empty;
        private string _oddEvenEven = string.Empty;
        
        // Half Time (Hiệp 1) - HDP
        private string _htHDPLine = string.Empty;
        private string _htHDPHome = string.Empty;
        private string _htHDPAway = string.Empty;
        
        // Half Time - OU
        private string _htOULine = string.Empty;
        private string _htOUOver = string.Empty;
        private string _htOUUnder = string.Empty;
        
        // Half Time - 1X2
        private string _ht1X2Home = string.Empty;
        private string _ht1X2Draw = string.Empty;
        private string _ht1X2Away = string.Empty;

        public int EventId { get; set; }
        public int LeagueId { get; set; }
        public int MatchStatsId { get; set; }
        public double StakeAmount { get; set; }
        public bool IsFirstRowOfMatch { get; set; }
        public bool IsLiveMatch { get; set; }

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

        // Full Time Properties
        public string FTHDPLine
        {
            get => _ftHDPLine;
            set { _ftHDPLine = value; OnPropertyChanged(nameof(FTHDPLine)); }
        }

        public string FTHDPHome
        {
            get => _ftHDPHome;
            set { _ftHDPHome = value; OnPropertyChanged(nameof(FTHDPHome)); }
        }

        public string FTHDPAway
        {
            get => _ftHDPAway;
            set { _ftHDPAway = value; OnPropertyChanged(nameof(FTHDPAway)); }
        }

        public string FTOULine
        {
            get => _ftOULine;
            set { _ftOULine = value; OnPropertyChanged(nameof(FTOULine)); }
        }

        public string FTOUOver
        {
            get => _ftOUOver;
            set { _ftOUOver = value; OnPropertyChanged(nameof(FTOUOver)); }
        }

        public string FTOUUnder
        {
            get => _ftOUUnder;
            set { _ftOUUnder = value; OnPropertyChanged(nameof(FTOUUnder)); }
        }

        public string FT1X2Home
        {
            get => _ft1X2Home;
            set { _ft1X2Home = value; OnPropertyChanged(nameof(FT1X2Home)); }
        }

        public string FT1X2Draw
        {
            get => _ft1X2Draw;
            set { _ft1X2Draw = value; OnPropertyChanged(nameof(FT1X2Draw)); }
        }

        public string FT1X2Away
        {
            get => _ft1X2Away;
            set { _ft1X2Away = value; OnPropertyChanged(nameof(FT1X2Away)); }
        }

        // Odd/Even Properties
        public string OddEvenOdd
        {
            get => _oddEvenOdd;
            set { _oddEvenOdd = value; OnPropertyChanged(nameof(OddEvenOdd)); }
        }

        public string OddEvenEven
        {
            get => _oddEvenEven;
            set { _oddEvenEven = value; OnPropertyChanged(nameof(OddEvenEven)); }
        }

        // Half Time Properties
        public string HTHDPLine
        {
            get => _htHDPLine;
            set { _htHDPLine = value; OnPropertyChanged(nameof(HTHDPLine)); }
        }

        public string HTHDPHome
        {
            get => _htHDPHome;
            set { _htHDPHome = value; OnPropertyChanged(nameof(HTHDPHome)); }
        }

        public string HTHDPAway
        {
            get => _htHDPAway;
            set { _htHDPAway = value; OnPropertyChanged(nameof(HTHDPAway)); }
        }

        public string HTOULine
        {
            get => _htOULine;
            set { _htOULine = value; OnPropertyChanged(nameof(HTOULine)); }
        }

        public string HTOUOver
        {
            get => _htOUOver;
            set { _htOUOver = value; OnPropertyChanged(nameof(HTOUOver)); }
        }

        public string HTOUUnder
        {
            get => _htOUUnder;
            set { _htOUUnder = value; OnPropertyChanged(nameof(HTOUUnder)); }
        }

        public string HT1X2Home
        {
            get => _ht1X2Home;
            set { _ht1X2Home = value; OnPropertyChanged(nameof(HT1X2Home)); }
        }

        public string HT1X2Draw
        {
            get => _ht1X2Draw;
            set { _ht1X2Draw = value; OnPropertyChanged(nameof(HT1X2Draw)); }
        }

        public string HT1X2Away
        {
            get => _ht1X2Away;
            set { _ht1X2Away = value; OnPropertyChanged(nameof(HT1X2Away)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
