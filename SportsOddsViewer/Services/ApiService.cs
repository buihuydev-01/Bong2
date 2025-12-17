using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SportsOddsViewer.Models;

namespace SportsOddsViewer.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx?od-param=2,1,1,1,1,2,1,2,0&fi=1&v=57783&dl=0";

        public ApiService()
        {
            _httpClient = new HttpClient();
            SetupHttpClient();
        }

        private void SetupHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("accept", "*/*");
            _httpClient.DefaultRequestHeaders.Add("accept-language", "vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5");
            _httpClient.DefaultRequestHeaders.Add("referer", "https://sports.wwyyuuvv22.com/web-root/restricted/default.aspx");
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36");
        }

        public async Task<List<Match>> FetchMatchesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_apiUrl);
                return ParseResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return new List<Match>();
            }
        }

        private List<Match> ParseResponse(string response)
        {
            var matches = new List<Match>();

            try
            {
                // Parse leagues
                var leaguesDict = new Dictionary<int, string>();
                var leaguePattern = @"\[(\d+),'([^']+)'";
                var leagueMatches = Regex.Matches(response, leaguePattern);
                
                foreach (Match leagueMatch in leagueMatches)
                {
                    if (int.TryParse(leagueMatch.Groups[1].Value, out int leagueId))
                    {
                        var leagueName = DecodeString(leagueMatch.Groups[2].Value);
                        leaguesDict[leagueId] = leagueName;
                    }
                }

                // Parse matches data
                // Pattern to find match arrays: [eventId,sportType,leagueId,'homeTeam','awayTeam','matchCode',...]
                var matchPattern = @"\[(\d+),1,(\d+),'([^']+)','([^']+)','([^']+)',(\d+),'([^']+)',";
                var matchMatches = Regex.Matches(response, matchPattern);

                foreach (Match m in matchMatches)
                {
                    try
                    {
                        var match = new Match
                        {
                            EventId = int.Parse(m.Groups[1].Value),
                            LeagueId = int.Parse(m.Groups[2].Value),
                            HomeTeam = DecodeString(m.Groups[3].Value),
                            AwayTeam = DecodeString(m.Groups[4].Value),
                            Time = ParseDateTime(m.Groups[7].Value),
                            Status = m.Groups[6].Value == "10" ? "Sắp diễn ra" : 
                                    m.Groups[6].Value == "6" ? "Live" :
                                    m.Groups[6].Value == "2" ? "Kết thúc" : ""
                        };

                        if (leaguesDict.TryGetValue(match.LeagueId, out string? leagueName))
                        {
                            match.League = leagueName;
                        }

                        matches.Add(match);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing match: {ex.Message}");
                    }
                }

                // Parse odds data
                ParseOdds(response, matches);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing response: {ex.Message}");
            }

            return matches;
        }

        private void ParseOdds(string response, List<Match> matches)
        {
            try
            {
                // Pattern to find odds data arrays
                // [oddsId,[eventId,betType,subType,...],[odds1,odds2]]
                var oddsPattern = @"\[\d+,\[(\d+),(\d+),\d+,[^\]]+\],\[([^\]]+)\]\]";
                var oddsMatches = Regex.Matches(response, oddsPattern);

                foreach (Match oddsMatch in oddsMatches)
                {
                    try
                    {
                        var eventId = int.Parse(oddsMatch.Groups[1].Value);
                        var betType = int.Parse(oddsMatch.Groups[2].Value);
                        var oddsValues = oddsMatch.Groups[3].Value.Split(',');

                        var match = matches.FirstOrDefault(m => m.EventId == eventId);
                        if (match == null) continue;

                        switch (betType)
                        {
                            case 1: // Handicap (HDP)
                                if (oddsValues.Length >= 2)
                                {
                                    match.OddsHDPHome = FormatOdds(oddsValues[0]);
                                    match.OddsHDPAway = FormatOdds(oddsValues[1]);
                                }
                                break;
                            case 3: // Over/Under (OU)
                                if (oddsValues.Length >= 2)
                                {
                                    match.OddsOUOver = FormatOdds(oddsValues[0]);
                                    match.OddsOUUnder = FormatOdds(oddsValues[1]);
                                }
                                break;
                            case 5: // 1X2
                                if (oddsValues.Length >= 3)
                                {
                                    match.Odds1X2Home = FormatOdds(oddsValues[0]);
                                    match.Odds1X2Draw = FormatOdds(oddsValues[1]);
                                    match.Odds1X2Away = FormatOdds(oddsValues[2]);
                                }
                                break;
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing odds: {ex.Message}");
            }
        }

        private string FormatOdds(string odds)
        {
            if (string.IsNullOrWhiteSpace(odds)) return "";
            
            if (double.TryParse(odds, out double value))
            {
                return value.ToString("F2");
            }
            return odds;
        }

        private string ParseDateTime(string dateStr)
        {
            try
            {
                // Format: "12/18/2025 01:00"
                if (DateTime.TryParse(dateStr, out DateTime dateTime))
                {
                    var now = DateTime.Now;
                    var diff = dateTime - now;

                    if (Math.Abs(diff.TotalHours) < 24)
                    {
                        return dateTime.ToString("HH:mm");
                    }
                    return dateTime.ToString("dd/MM HH:mm");
                }
            }
            catch { }

            return dateStr;
        }

        private string DecodeString(string encoded)
        {
            try
            {
                // Decode hex escapes like \xC3 to proper characters
                var result = Regex.Replace(encoded, @"\\x([0-9A-Fa-f]{2})", match =>
                {
                    var hex = match.Groups[1].Value;
                    var value = Convert.ToInt32(hex, 16);
                    return ((char)value).ToString();
                });

                // Decode unicode escapes like \u1ED9
                result = Regex.Replace(result, @"\\u([0-9A-Fa-f]{4})", match =>
                {
                    var hex = match.Groups[1].Value;
                    var value = Convert.ToInt32(hex, 16);
                    return char.ConvertFromUtf32(value);
                });

                return result;
            }
            catch
            {
                return encoded;
            }
        }
    }
}
