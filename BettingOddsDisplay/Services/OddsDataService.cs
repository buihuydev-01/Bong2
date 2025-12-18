using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BettingOddsDisplay.Models;

namespace BettingOddsDisplay.Services
{
    public class OddsDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ResponseParser _parser;
        private Timer? _timer;
        private bool _isRunning;

        public event Action<BettingData>? DataUpdated;
        public event Action<string>? ErrorOccurred;

        public OddsDataService()
        {
            _httpClient = new HttpClient();
            _parser = new ResponseParser();
            SetupHttpClient();
        }

        private void SetupHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("accept", "*/*");
            _httpClient.DefaultRequestHeaders.Add("accept-language", "vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5");
            _httpClient.DefaultRequestHeaders.Add("priority", "u=1, i");
            _httpClient.DefaultRequestHeaders.Add("referer", "https://sports.wwyyuuvv22.com/web-root/restricted/default.aspx?loginname=4fc5d99e25177dcb71aa4e4974e4d74b&lang=VI_VN&oddstyle=MY&theme=sbo&oddsmode=double&jd=jd&u=10028yy_v86h1831617sbd&in=0");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"143\", \"Chromium\";v=\"143\", \"Not A(Brand)\";v=\"24\"");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-storage-access", "active");
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36");
            
            // Add cookies
            _httpClient.DefaultRequestHeaders.Add("Cookie", 
                "ASP.NET_SessionId=thj4vns4z2mckcjz5rzaopqc; " +
                "_hjSessionUser_1325134=eyJpZCI6ImVjMWE4OTkwLTZhMDAtNTNjYi05MmM2LWEwMmE1Njg1YjNiNyIsImNyZWF0ZWQiOjE3NjU5NTQ4MDkwNDUsImV4aXN0aW5nIjp0cnVlfQ==; " +
                "fullScreenAds=true; " +
                "favorites%3A10028yy_v86h1831617sbd=[{%22sportType%22:1%2C%22eventIds%22:[9197785%2C9197786%2C9197787%2C9197788%2C9197789%2C9197790%2C9197791%2C9197792%2C9197793%2C9197794]%2C%22leagueIds%22:[]}]; " +
                "_hjSession_1325134=eyJpZCI6IjFlMDUwNTE3LTEwMTctNGZhYy1hZDQyLWIwNWNmMWVhOGY4MCIsImMiOjE3NjYwMjYxMTMxOTgsInMiOjAsInIiOjAsInNiIjowLCJzciI6MCwic2UiOjAsImZzIjowLCJzcCI6MH0=; " +
                "states=:1:1:::1::::::::::1766026116859:1766026116862:1766026116863:1766026116864:1766026116864");
        }

        public void Start()
        {
            if (_isRunning) return;
            
            _isRunning = true;
            
            // Fetch immediately
            _ = FetchDataAsync();
            
            // Then fetch every 2 seconds (slower to avoid blocking)
            _timer = new Timer(async _ => await FetchDataAsync(), null, 2000, 2000);
        }

        public void Stop()
        {
            _isRunning = false;
            _timer?.Dispose();
            _timer = null;
        }

        private async Task FetchDataAsync()
        {
            if (!_isRunning) return;

            try
            {
                var url = "https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx?od-param=2,1,1,1,1,2,1,2,0&fi=1&v=131885&dl=0";
                
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Fetching data from API...");
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Response length: {response.Length} chars");
                
                var data = _parser.ParseResponse(response);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Parsed: {data.Leagues.Count} leagues, {data.Matches.Count} matches");
                
                DataUpdated?.Invoke(data);
            }
            catch (HttpRequestException ex)
            {
                ErrorOccurred?.Invoke($"Network error: {ex.Message}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Error: {ex.Message}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        public void Dispose()
        {
            Stop();
            _httpClient?.Dispose();
        }
    }
}
