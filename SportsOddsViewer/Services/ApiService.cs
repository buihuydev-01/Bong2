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
                // [oddsId,[eventId,betType,subType,handicapValue,...],[odds1,odds2,...]]
                var oddsPattern = @"\[\d+,\[(\d+),(\d+),(\d+),([^,\]]+),[^\]]*\],\[([^\]]+)\]\]";
                var oddsMatches = Regex.Matches(response, oddsPattern);

                foreach (Match oddsMatch in oddsMatches)
                {
                    try
                    {
                        var eventId = int.Parse(oddsMatch.Groups[1].Value);
                        var betType = int.Parse(oddsMatch.Groups[2].Value);
                        var subType = int.Parse(oddsMatch.Groups[3].Value);
                        var handicapValue = oddsMatch.Groups[4].Value;
                        var oddsValues = oddsMatch.Groups[5].Value.Split(',');

                        var match = matches.FirstOrDefault(m => m.EventId == eventId);
                        if (match == null) continue;

                        // betType: 1=HDP, 3=OU, 5=1X2, 7=HT HDP, 8=HT 1X2, 9=HT OU, 12=Odd/Even
                        
                        switch (betType)
                        {
                            case 1: // Full Time Handicap (HDP)
                                if (oddsValues.Length >= 2)
                                {
                                    var hdpLine = FormatHandicap(handicapValue);
                                    match.FTHDPLine = hdpLine;
                                    match.FTHDPHome = FormatOdds(oddsValues[0]);
                                    match.FTHDPLineAway = FormatHandicapOpposite(handicapValue);
                                    match.FTHDPAway = FormatOdds(oddsValues[1]);
                                }
                                break;
                                
                            case 3: // Full Time Over/Under (OU)
                                if (oddsValues.Length >= 2)
                                {
                                    var ouLine = handicapValue;
                                    match.FTOULine = ouLine;
                                    match.FTOUOver = FormatOdds(oddsValues[0]);
                                    match.FTOULineUnder = ouLine;
                                    match.FTOUUnder = FormatOdds(oddsValues[1]);
                                }
                                break;
                                
                            case 5: // Full Time 1X2
                                if (oddsValues.Length >= 3)
                                {
                                    match.FT1X2Home = FormatOdds(oddsValues[0]);
                                    match.FT1X2Draw = FormatOdds(oddsValues[1]);
                                    match.FT1X2Away = FormatOdds(oddsValues[2]);
                                }
                                break;
                                
                            case 7: // Half Time Handicap (HDP)
                                if (oddsValues.Length >= 2)
                                {
                                    var hdpLine = FormatHandicap(handicapValue);
                                    match.HTHDPLine = hdpLine;
                                    match.HTHDPHome = FormatOdds(oddsValues[0]);
                                    match.HTHDPLineAway = FormatHandicapOpposite(handicapValue);
                                    match.HTHDPAway = FormatOdds(oddsValues[1]);
                                }
                                break;
                                
                            case 8: // Half Time 1X2
                                if (oddsValues.Length >= 3)
                                {
                                    match.HT1X2Home = FormatOdds(oddsValues[0]);
                                    match.HT1X2Draw = FormatOdds(oddsValues[1]);
                                    match.HT1X2Away = FormatOdds(oddsValues[2]);
                                }
                                break;
                                
                            case 9: // Half Time Over/Under (OU)
                                if (oddsValues.Length >= 2)
                                {
                                    var ouLine = handicapValue;
                                    match.HTOULine = ouLine;
                                    match.HTOUOver = FormatOdds(oddsValues[0]);
                                    match.HTOULineUnder = ouLine;
                                    match.HTOUUnder = FormatOdds(oddsValues[1]);
                                }
                                break;
                                
                            case 12: // Odd/Even
                                if (oddsValues.Length >= 2)
                                {
                                    match.OddEvenOdd = FormatOdds(oddsValues[0]);
                                    match.OddEvenEven = FormatOdds(oddsValues[1]);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing individual odds: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing odds: {ex.Message}");
            }
        }

        private string FormatHandicap(string handicap)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(handicap)) return "";
                
                if (double.TryParse(handicap, out double value))
                {
                    if (value > 0)
                        return $"+{value:F2}";
                    else if (value < 0)
                        return value.ToString("F2");
                    else
                        return "0.00";
                }
            }
            catch { }
            return handicap;
        }

        private string FormatHandicapOpposite(string handicap)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(handicap)) return "";
                
                if (double.TryParse(handicap, out double value))
                {
                    value = -value;
                    if (value > 0)
                        return $"+{value:F2}";
                    else if (value < 0)
                        return value.ToString("F2");
                    else
                        return "0.00";
                }
            }
            catch { }
            return handicap;
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
