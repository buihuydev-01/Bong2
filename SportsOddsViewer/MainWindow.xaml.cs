using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using SportsOddsViewer.Models;
using SportsOddsViewer.Services;

namespace SportsOddsViewer
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly DispatcherTimer _timer;
        private readonly ObservableCollection<Match> _matches;
        private bool _isUpdating = false;

        public MainWindow()
        {
            InitializeComponent();

            _apiService = new ApiService();
            _matches = new ObservableCollection<Match>();
            MatchesDataGrid.ItemsSource = _matches;

            // Setup timer for auto-update every 1 second
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Initial load
            _ = LoadDataAsync();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            if (AutoUpdateToggle.IsChecked == true && !_isUpdating)
            {
                await LoadDataAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            if (_isUpdating) return;

            _isUpdating = true;
            StatusText.Text = "Đang tải dữ liệu...";

            try
            {
                var newMatches = await _apiService.FetchMatchesAsync();

                if (newMatches.Count > 0)
                {
                    // Update existing matches or add new ones
                    foreach (var newMatch in newMatches)
                    {
                        var existingMatch = _matches.FirstOrDefault(m => m.EventId == newMatch.EventId);
                        
                        if (existingMatch != null)
                        {
                            // Update existing match
                            UpdateMatch(existingMatch, newMatch);
                        }
                        else
                        {
                            // Add new match
                            _matches.Add(newMatch);
                        }
                    }

                    // Remove matches that are no longer in the API response
                    var matchesToRemove = _matches
                        .Where(m => !newMatches.Any(nm => nm.EventId == m.EventId))
                        .ToList();

                    foreach (var match in matchesToRemove)
                    {
                        _matches.Remove(match);
                    }

                    // Sort by time
                    var sortedMatches = _matches.OrderBy(m => m.Time).ToList();
                    _matches.Clear();
                    foreach (var match in sortedMatches)
                    {
                        _matches.Add(match);
                    }

                    LastUpdateText.Text = $"Cập nhật lúc: {DateTime.Now:HH:mm:ss}";
                    StatusText.Text = "✓ Kết nối thành công";
                    MatchCountText.Text = $"Số trận: {_matches.Count}";
                }
                else
                {
                    StatusText.Text = "Không có dữ liệu";
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"❌ Lỗi: {ex.Message}";
                LastUpdateText.Text = $"Lỗi lúc: {DateTime.Now:HH:mm:ss}";
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private void UpdateMatch(Match existing, Match newData)
        {
            // Basic info
            existing.Time = newData.Time;
            existing.Status = newData.Status;
            existing.HomeTeam = newData.HomeTeam;
            existing.AwayTeam = newData.AwayTeam;
            existing.League = newData.League;
            existing.Score = newData.Score;
            
            // Full Time (Nguyên trận) - HDP
            existing.FTHDPLine = newData.FTHDPLine;
            existing.FTHDPHome = newData.FTHDPHome;
            existing.FTHDPLineAway = newData.FTHDPLineAway;
            existing.FTHDPAway = newData.FTHDPAway;
            
            // Full Time - OU
            existing.FTOULine = newData.FTOULine;
            existing.FTOUOver = newData.FTOUOver;
            existing.FTOULineUnder = newData.FTOULineUnder;
            existing.FTOUUnder = newData.FTOUUnder;
            
            // Full Time - 1X2
            existing.FT1X2Home = newData.FT1X2Home;
            existing.FT1X2Draw = newData.FT1X2Draw;
            existing.FT1X2Away = newData.FT1X2Away;
            
            // Half Time (Hiệp 1) - HDP
            existing.HTHDPLine = newData.HTHDPLine;
            existing.HTHDPHome = newData.HTHDPHome;
            existing.HTHDPLineAway = newData.HTHDPLineAway;
            existing.HTHDPAway = newData.HTHDPAway;
            
            // Half Time - OU
            existing.HTOULine = newData.HTOULine;
            existing.HTOUOver = newData.HTOUOver;
            existing.HTOULineUnder = newData.HTOULineUnder;
            existing.HTOUUnder = newData.HTOUUnder;
            
            // Half Time - 1X2
            existing.HT1X2Home = newData.HT1X2Home;
            existing.HT1X2Draw = newData.HT1X2Draw;
            existing.HT1X2Away = newData.HT1X2Away;
            
            // Odd/Even
            existing.OddEvenOdd = newData.OddEvenOdd;
            existing.OddEvenEven = newData.OddEvenEven;
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private void AutoUpdateToggle_Click(object sender, RoutedEventArgs e)
        {
            if (AutoUpdateToggle.IsChecked == true)
            {
                AutoUpdateToggle.Content = "⏸ Tạm dừng";
                AutoUpdateToggle.Background = System.Windows.Media.Brushes.Red;
                StatusText.Text = "Tự động cập nhật: BẬT";
            }
            else
            {
                AutoUpdateToggle.Content = "▶ Tiếp tục";
                AutoUpdateToggle.Background = System.Windows.Media.Brushes.Green;
                StatusText.Text = "Tự động cập nhật: TẮT";
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            base.OnClosed(e);
        }
    }
}
