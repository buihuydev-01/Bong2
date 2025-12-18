using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BettingOddsDisplay.Models;
using BettingOddsDisplay.Services;
using BettingOddsDisplay.ViewModels;

namespace BettingOddsDisplay
{
    public partial class MainWindow : Window
    {
        private readonly OddsDataService _dataService;
        private ObservableCollection<LeagueViewModel> _leagues;

        public MainWindow()
        {
            InitializeComponent();
            
            _leagues = new ObservableCollection<LeagueViewModel>();
            LeaguesItemsControl.ItemsSource = _leagues;
            
            _dataService = new OddsDataService();
            _dataService.DataUpdated += OnDataUpdated;
            _dataService.ErrorOccurred += OnErrorOccurred;
            
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Đang kết nối...";
            Console.WriteLine("=== Betting Odds Display Started ===");
            Console.WriteLine($"Time: {DateTime.Now}");
            
            // Test with sample data first
            TestWithSampleData();
            
            // Then start real service
            _dataService.Start();
        }
        
        private void TestWithSampleData()
        {
            try
            {
                var sampleFile = "sample_response.txt";
                if (System.IO.File.Exists(sampleFile))
                {
                    Console.WriteLine("[TEST] Loading sample data...");
                    var response = System.IO.File.ReadAllText(sampleFile);
                    var parser = new Services.ResponseParser();
                    var data = parser.ParseResponse(response);
                    
                    Console.WriteLine($"[TEST] Sample data parsed: {data.Leagues.Count} leagues, {data.Matches.Count} matches");
                    
                    // Display sample data
                    OnDataUpdated(data);
                }
                else
                {
                    Console.WriteLine("[TEST] sample_response.txt not found, skipping test");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TEST] Error: {ex.Message}");
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _dataService.Stop();
            _dataService.Dispose();
        }

        private void OnDataUpdated(BettingData data)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    Console.WriteLine($"[UI] Updating UI with {data.Matches.Count} matches");
                    UpdateTimeText.Text = data.UpdateTime;
                    StatusText.Text = $"Đã tải {data.Matches.Count} trận đấu từ {data.Leagues.Count} giải đấu";
                    
                    UpdateLeagueData(data);
                    Console.WriteLine($"[UI] UI updated successfully");
                }
                catch (Exception ex)
                {
                    StatusText.Text = $"Lỗi cập nhật UI: {ex.Message}";
                    Console.WriteLine($"[UI] Error: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            });
        }

        private void UpdateLeagueData(BettingData data)
        {
            // Group matches by league
            var matchesByLeague = data.Matches
                .GroupBy(m => m.LeagueId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Clear existing leagues
            _leagues.Clear();

            // Add leagues with their matches
            foreach (var league in data.Leagues)
            {
                if (matchesByLeague.TryGetValue(league.LeagueId, out var matches))
                {
                    var leagueVM = new LeagueViewModel
                    {
                        Name = league.Name,
                        SubLeague = league.SubLeague,
                        MatchInfo = league.MatchInfo
                    };

                    foreach (var match in matches)
                    {
                        leagueVM.Matches.Add(match);
                    }

                    _leagues.Add(leagueVM);
                }
            }
        }

        private void OnErrorOccurred(string error)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = error;
                Console.WriteLine($"[ERROR] {error}");
            });
        }
    }
}
