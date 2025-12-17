using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MatchModel = SportsOddsViewer.Models.Match;
using SportsOddsViewer.Services;

namespace SportsOddsViewer
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly DispatcherTimer _timer;
        private readonly ObservableCollection<MatchModel> _matches;
        private bool _isUpdating = false;

        public MainWindow()
        {
            InitializeComponent();

            _apiService = new ApiService();
            _matches = new ObservableCollection<MatchModel>();
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
                    // Build a dictionary of new matches by key (eventId_stake)
                    var newMatchDict = new Dictionary<string, MatchModel>();
                    foreach (var m in newMatches)
                    {
                        newMatchDict[GetMatchKey(m)] = m;
                    }
                    
                    // Remove matches that are no longer in the API response
                    var matchesToRemove = _matches
                        .Where(m => !newMatchDict.ContainsKey(GetMatchKey(m)))
                        .ToList();

                    foreach (var match in matchesToRemove)
                    {
                        _matches.Remove(match);
                    }
                    
                    // Update existing matches
                    foreach (var existing in _matches.ToList())
                    {
                        var key = GetMatchKey(existing);
                        if (newMatchDict.TryGetValue(key, out var newMatch))
                        {
                            UpdateMatch(existing, newMatch);
                            newMatchDict.Remove(key); // Mark as processed
                        }
                    }
                    
                    // Add new matches (ones not yet in _matches)
                    foreach (var newMatch in newMatchDict.Values)
                    {
                        _matches.Add(newMatch);
                    }

                    LastUpdateText.Text = $"Cập nhật lúc: {DateTime.Now:HH:mm:ss}";
                    StatusText.Text = "✓ Kết nối thành công";
                    
                    // Count unique matches (by EventId)
                    var uniqueMatches = _matches.Select(m => m.EventId).Distinct().Count();
                    var totalRows = _matches.Count;
                    MatchCountText.Text = $"Số trận: {uniqueMatches} ({totalRows} dòng)";
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

        private string GetMatchKey(MatchModel match)
        {
            return $"{match.EventId}_{match.StakeAmount:F2}";
        }

        private bool HasMatchChanged(MatchModel existing, MatchModel newData)
        {
            // Chỉ check các fields quan trọng để detect thay đổi
            return existing.FTHDPLine != newData.FTHDPLine ||
                   existing.FTHDPHome != newData.FTHDPHome ||
                   existing.FTHDPAway != newData.FTHDPAway ||
                   existing.FTOULine != newData.FTOULine ||
                   existing.FTOUOver != newData.FTOUOver ||
                   existing.FTOUUnder != newData.FTOUUnder ||
                   existing.FT1X2Home != newData.FT1X2Home ||
                   existing.FT1X2Draw != newData.FT1X2Draw ||
                   existing.FT1X2Away != newData.FT1X2Away ||
                   existing.HTHDPLine != newData.HTHDPLine ||
                   existing.HTHDPHome != newData.HTHDPHome ||
                   existing.HTHDPAway != newData.HTHDPAway ||
                   existing.HTOULine != newData.HTOULine ||
                   existing.HTOUOver != newData.HTOUOver ||
                   existing.HTOUUnder != newData.HTOUUnder ||
                   existing.HT1X2Home != newData.HT1X2Home ||
                   existing.HT1X2Draw != newData.HT1X2Draw ||
                   existing.HT1X2Away != newData.HT1X2Away ||
                   existing.OddEvenOdd != newData.OddEvenOdd ||
                   existing.OddEvenEven != newData.OddEvenEven;
        }

        private void UpdateMatch(MatchModel existing, MatchModel newData)
        {
            // CHỈ UPDATE NẾU CÓ THAY ĐỔI - tránh lag
            if (!HasMatchChanged(existing, newData))
            {
                return; // Không có gì thay đổi, skip update
            }

            // Basic info
            existing.Time = newData.Time;
            existing.Status = newData.Status;
            existing.HomeTeam = newData.HomeTeam;
            existing.AwayTeam = newData.AwayTeam;
            existing.League = newData.League;
            existing.IsFirstRowOfMatch = newData.IsFirstRowOfMatch;
            existing.StakeAmount = newData.StakeAmount;
            
            // Full Time (Nguyên trận) - HDP
            existing.FTHDPLine = newData.FTHDPLine;
            existing.FTHDPHome = newData.FTHDPHome;
            existing.FTHDPAway = newData.FTHDPAway;
            
            // Full Time - OU
            existing.FTOULine = newData.FTOULine;
            existing.FTOUOver = newData.FTOUOver;
            existing.FTOUUnder = newData.FTOUUnder;
            
            // Full Time - 1X2
            existing.FT1X2Home = newData.FT1X2Home;
            existing.FT1X2Draw = newData.FT1X2Draw;
            existing.FT1X2Away = newData.FT1X2Away;
            
            // Half Time (Hiệp 1) - HDP
            existing.HTHDPLine = newData.HTHDPLine;
            existing.HTHDPHome = newData.HTHDPHome;
            existing.HTHDPAway = newData.HTHDPAway;
            
            // Half Time - OU
            existing.HTOULine = newData.HTOULine;
            existing.HTOUOver = newData.HTOUOver;
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
