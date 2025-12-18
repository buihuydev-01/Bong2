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
        private ObservableCollection<Models.Match> _matches;

        public MainWindow()
        {
            InitializeComponent();
            
            _matches = new ObservableCollection<Models.Match>();
            MatchesItemsControl.ItemsSource = _matches;
            
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
                // Test với response thật từ user
                Console.WriteLine("[TEST] Testing with real response...");
                var realResponse = @"$M('odds-display').onUpdate(2,[82139,1,1,[[[39,'Cúp Tây Ban Nha','',''],[427465,'e-Football F24 Elite Club Friendly','','2 x 8 minutes'],[427468,'e-Football F24 International Friendly','','2 x 8 minutes']],[[9202091,1,427468,'e-Finland','e-Spain','0050-E3130251218007',8,'12/18/2025 12:45',0,'twitch',19,0,0,0],[9202092,1,427468,'e-France','e-Italy','0050-E3130251218009',8,'12/18/2025 13:00',0,'twitch',19,0,0,0],[9202093,1,427468,'e-Denmark','e-Germany','0050-E3130251218010',8,'12/18/2025 13:00',0,'twitch',19,0,0,0]],[[113866838,9202091,0,0,0,19],[113866841,9202092,0,0,0,19],[113866842,9202093,0,0,0,19]],,,[[5739590030,[113866838,1,0,500.00,-1.00],[0.88,0.84]],[5739590040,[113866838,1,0,300.00,-1.25],[0.63,0.81]],[5739590050,[113866838,1,0,200.00,-0.75],[-0.88,0.60]],[5739590150,[113866838,2,0,200.00,0.00],[0.86,0.86]],[5739590090,[113866838,3,0,500.00,3.00],[0.80,0.92]],[5739590100,[113866838,3,0,300.00,3.25],[0.91,0.67]],[5739590110,[113866838,3,0,200.00,2.75],[0.58,-0.86]],[5739590160,[113866838,5,0,200.00,0],[4.64,3.88,1.47]],[5739590060,[113866838,7,0,300.00,-0.50],[0.74,0.84]],[5739590070,[113866838,7,0,200.00,-0.25],[-0.87,0.59]],[5739590080,[113866838,7,0,100.00,-0.75],[0.45,-0.87]],[5739590170,[113866838,8,0,200.00,0],[4.24,2.41,2.00]],[5739590120,[113866838,9,0,300.00,1.25],[0.66,0.92]],[5739590130,[113866838,9,0,200.00,1.50],[-0.86,0.58]],[5739590140,[113866838,9,0,100.00,1.00],[0.38,-0.66]],[5739590590,[113866841,1,0,500.00,1.00],[0.80,0.92]],[5739590600,[113866841,1,0,300.00,1.25],[-0.98,0.70]],[5739590610,[113866841,1,0,200.00,0.75],[0.61,-0.89]],[5739590750,[113866841,2,0,200.00,0.00],[0.86,0.86]],[5739590680,[113866841,3,0,500.00,4.25],[0.74,0.98]],[5739590660,[113866841,3,0,300.00,4.50],[0.80,0.78]],[5739590690,[113866841,3,0,200.00,4.75],[-0.88,0.60]],[5739590760,[113866841,5,0,200.00,0],[1.50,4.41,3.86]],[5739590620,[113866841,7,0,300.00,0.25],[0.74,0.98]],[5739590630,[113866841,7,0,200.00,0.50],[-0.96,0.68]],[5739590650,[113866841,7,0,100.00,0.75],[-0.71,0.43]],[5739590770,[113866841,8,0,200.00,0],[2.06,2.84,3.18]],[5739590710,[113866841,9,0,300.00,2.00],[0.95,0.77]],[5739590720,[113866841,9,0,200.00,1.75],[0.64,-0.92]],[5739590730,[113866841,9,0,100.00,2.25],[-0.79,0.51]],[5739591070,[113866842,1,0,500.00,0.25],[0.77,0.95]],[5739591080,[113866842,1,0,300.00,0.50],[0.99,0.73]],[5739591090,[113866842,1,0,200.00,0.75],[-0.81,0.53]],[5739591190,[113866842,2,0,200.00,0.00],[0.87,0.85]],[5739591130,[113866842,3,0,500.00,3.50],[0.76,0.96]],[5739591140,[113866842,3,0,300.00,3.75],[0.97,0.75]],[5739591150,[113866842,3,0,200.00,3.25],[0.41,-0.83]],[5739591200,[113866842,5,0,200.00,0],[2.01,3.55,2.69]],[5739591100,[113866842,7,0,300.00,0.25],[-0.97,0.69]],[5739591110,[113866842,7,0,200.00,0.00],[0.59,-0.87]],[5739591120,[113866842,7,0,100.00,0.50],[-0.72,0.44]],[5739591210,[113866842,8,0,200.00,0],[2.42,2.57,2.86]],[5739591160,[113866842,9,0,300.00,1.50],[0.80,0.92]],[5739591170,[113866842,9,0,200.00,1.75],[0.95,0.63]],[5739591180,[113866842,9,0,100.00,1.25],[0.48,-0.76]]],,,'Bóng Đá',0],,,,0]);";
                
                var parser = new Services.ResponseParser();
                var data = parser.ParseResponse(realResponse);
                
                Console.WriteLine($"[TEST] Real response parsed: {data.Leagues.Count} leagues, {data.Matches.Count} matches");
                
                if (data.Matches.Count > 0)
                {
                    Console.WriteLine($"[TEST] First match: {data.Matches[0].HomeTeam} vs {data.Matches[0].AwayTeam}");
                    Console.WriteLine($"[TEST] First match odds groups: {data.Matches[0].OddsGroups.Count}");
                    
                    foreach (var group in data.Matches[0].OddsGroups)
                    {
                        Console.WriteLine($"[TEST]   BetType {group.BetType}: {group.Lines.Count} lines");
                    }
                }
                
                // Display data
                if (data.Leagues.Count > 0 || data.Matches.Count > 0)
                {
                    OnDataUpdated(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TEST] Error: {ex.Message}");
                Console.WriteLine($"[TEST] Stack trace: {ex.StackTrace}");
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
            Console.WriteLine($"[UI] Clearing {_matches.Count} old matches");
            _matches.Clear();

            Console.WriteLine($"[UI] Adding {data.Matches.Count} new matches");
            foreach (var match in data.Matches)
            {
                Console.WriteLine($"[UI]   - {match.HomeTeam} vs {match.AwayTeam}, Odds groups: {match.OddsGroups.Count}");
                _matches.Add(match);
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
