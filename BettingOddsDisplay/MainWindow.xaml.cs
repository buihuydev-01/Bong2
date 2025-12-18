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
            _dataService.Start();
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
                    UpdateTimeText.Text = data.UpdateTime;
                    StatusText.Text = $"Đã tải {data.Matches.Count} trận đấu từ {data.Leagues.Count} giải đấu";
                    
                    UpdateLeagueData(data);
                }
                catch (Exception ex)
                {
                    StatusText.Text = $"Lỗi cập nhật UI: {ex.Message}";
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
            });
        }
    }
}
