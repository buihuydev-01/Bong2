using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MatchModel = SportsOddsViewer.Models.Match;

namespace SportsOddsViewer.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx?od-param=2,1,1,1,1,2,1,2,0&fi=1&v=64862&dl=0";

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

        public async Task<List<MatchModel>> FetchMatchesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_apiUrl);
                return ParseResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return new List<MatchModel>();
            }
        }

        private List<MatchModel> ParseResponse(string response)
        {
            var matchRows = new List<MatchModel>();

            try
            {
                // Response format: $M('odds-display').onUpdate(2,[id,type,flag,[[leagues],[matches],[stats],,,[odds]],...]);
                
                // 1. Parse leagues - trong array đầu tiên
                var leaguesDict = new Dictionary<int, string>();
                var leaguePattern = @"\[(\d+),'([^']*)'";
                var leagueMatches = Regex.Matches(response, leaguePattern);
                
                foreach (Match lm in leagueMatches)
                {
                    if (int.TryParse(lm.Groups[1].Value, out int leagueId))
                    {
                        var leagueName = DecodeString(lm.Groups[2].Value);
                        if (!leaguesDict.ContainsKey(leagueId))
                        {
                            leaguesDict[leagueId] = leagueName;
                        }
                    }
                }

                // 2. Parse matches - lấy thông tin cơ bản
                var matchesDict = new Dictionary<int, (string home, string away, int leagueId, string time, string status)>();
                var matchPattern = @"\[(\d+),1,(\d+),'([^']*)','([^']*)','[^']*',(\d+),'([^']*)',";
                var matchMatches = Regex.Matches(response, matchPattern);

                foreach (Match mm in matchMatches)
                {
                    try
                    {
                        var eventId = int.Parse(mm.Groups[1].Value);
                        var leagueId = int.Parse(mm.Groups[2].Value);
                        var homeTeam = DecodeString(mm.Groups[3].Value);
                        var awayTeam = DecodeString(mm.Groups[4].Value);
                        var statusCode = mm.Groups[5].Value;
                        var dateTime = mm.Groups[6].Value;

                        // Chỉ lấy trận KHÔNG phải Live (status code != 6 và != 8)
                        if (statusCode == "6" || statusCode == "8")
                        {
                            continue;
                        }

                        matchesDict[eventId] = (
                            homeTeam, 
                            awayTeam, 
                            leagueId, 
                            ParseDateTime(dateTime),
                            ParseStatus(statusCode)
                        );
                    }
                    catch { }
                }

                // 3. Parse match stats mapping: [matchStatsId, eventId, ...]
                var statsToEventMap = new Dictionary<int, int>();
                var statsPattern = @"\[(\d+),(\d+),\d+,\d+,\d+,\d+\]";
                var statsMatches = Regex.Matches(response, statsPattern);
                
                foreach (Match sm in statsMatches)
                {
                    if (int.TryParse(sm.Groups[1].Value, out int statsId) &&
                        int.TryParse(sm.Groups[2].Value, out int eventId))
                    {
                        statsToEventMap[statsId] = eventId;
                    }
                }

                // 4. Parse odds - mỗi entry tạo một row
                var oddsPattern = @"\[(\d+),\[(\d+),(\d+),(\d+),(\d+\.\d+),([^\]]+)\],\[([^\]]+)\]\]";
                var oddsMatches = Regex.Matches(response, oddsPattern);

                // Group odds by eventId and stake to create rows
                var oddsGroups = new Dictionary<string, MatchModel>();

                foreach (Match om in oddsMatches)
                {
                    try
                    {
                        if (!int.TryParse(om.Groups[2].Value, out int matchStatsId)) continue;
                        if (!int.TryParse(om.Groups[3].Value, out int betType)) continue;
                        if (!int.TryParse(om.Groups[4].Value, out int subType)) continue;
                        if (!double.TryParse(om.Groups[5].Value, out double stake)) continue;
                        if (!double.TryParse(om.Groups[6].Value, out double handicap)) continue;
                        
                        var oddsStr = om.Groups[7].Value;
                        var oddsValues = oddsStr.Split(',');

                        // Get eventId from statsId
                        if (!statsToEventMap.TryGetValue(matchStatsId, out int eventId)) continue;
                        if (!matchesDict.ContainsKey(eventId)) continue;

                        // Chỉ lấy odds chính (subType = 0)
                        if (subType != 0) continue;

                        // Key: eventId + stake để tạo unique row
                        var key = $"{eventId}_{stake:F2}";

                        if (!oddsGroups.ContainsKey(key))
                        {
                            var matchInfo = matchesDict[eventId];
                            var row = new MatchModel
                            {
                                EventId = eventId,
                                MatchStatsId = matchStatsId,
                                LeagueId = matchInfo.leagueId,
                                HomeTeam = matchInfo.home,
                                AwayTeam = matchInfo.away,
                                Time = matchInfo.time,
                                Status = matchInfo.status,
                                StakeAmount = stake
                            };

                            if (leaguesDict.TryGetValue(matchInfo.leagueId, out string? leagueName))
                            {
                                row.League = leagueName;
                            }

                            oddsGroups[key] = row;
                        }

                        var matchRow = oddsGroups[key];

                        // Parse odds theo betType
                        switch (betType)
                        {
                            case 1: // Full Time Handicap
                                if (oddsValues.Length >= 2)
                                {
                                    matchRow.FTHDPLine = FormatHandicap(handicap);
                                    matchRow.FTHDPHome = FormatOdds(oddsValues[0]);
                                    matchRow.FTHDPAway = FormatOdds(oddsValues[1]);
                                }
                                break;

                            case 3: // Full Time Over/Under
                                if (oddsValues.Length >= 2)
                                {
                                    matchRow.FTOULine = FormatLine(handicap);
                                    matchRow.FTOUOver = FormatOdds(oddsValues[0]);
                                    matchRow.FTOUUnder = FormatOdds(oddsValues[1]);
                                }
                                break;

                            case 5: // Full Time 1X2
                                if (oddsValues.Length >= 3)
                                {
                                    matchRow.FT1X2Home = FormatOdds(oddsValues[0]);
                                    matchRow.FT1X2Draw = FormatOdds(oddsValues[1]);
                                    matchRow.FT1X2Away = FormatOdds(oddsValues[2]);
                                }
                                break;

                            case 7: // Half Time Handicap
                                if (oddsValues.Length >= 2)
                                {
                                    matchRow.HTHDPLine = FormatHandicap(handicap);
                                    matchRow.HTHDPHome = FormatOdds(oddsValues[0]);
                                    matchRow.HTHDPAway = FormatOdds(oddsValues[1]);
                                }
                                break;

                            case 9: // Half Time Over/Under
                                if (oddsValues.Length >= 2)
                                {
                                    matchRow.HTOULine = FormatLine(handicap);
                                    matchRow.HTOUOver = FormatOdds(oddsValues[0]);
                                    matchRow.HTOUUnder = FormatOdds(oddsValues[1]);
                                }
                                break;

                            case 8: // Half Time 1X2
                                if (oddsValues.Length >= 3)
                                {
                                    matchRow.HT1X2Home = FormatOdds(oddsValues[0]);
                                    matchRow.HT1X2Draw = FormatOdds(oddsValues[1]);
                                    matchRow.HT1X2Away = FormatOdds(oddsValues[2]);
                                }
                                break;

                            case 12: // Odd/Even
                                if (oddsValues.Length >= 2)
                                {
                                    matchRow.OddEvenOdd = FormatOdds(oddsValues[0]);
                                    matchRow.OddEvenEven = FormatOdds(oddsValues[1]);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing odds: {ex.Message}");
                    }
                }

                // Convert to list và sort
                matchRows = oddsGroups.Values
                    .OrderBy(m => m.Time)
                    .ThenBy(m => m.EventId)
                    .ThenByDescending(m => m.StakeAmount)
                    .ToList();

                // Đánh dấu row đầu tiên của mỗi trận
                int? currentEventId = null;
                foreach (var row in matchRows)
                {
                    if (currentEventId != row.EventId)
                    {
                        row.IsFirstRowOfMatch = true;
                        currentEventId = row.EventId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing response: {ex.Message}");
            }

            return matchRows;
        }

        private string FormatOdds(string odds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(odds)) return "";
                odds = odds.Trim();
                
                if (double.TryParse(odds, out double value))
                {
                    if (Math.Abs(value) < 0.01) return "";
                    
                    // Hiển thị màu đỏ nếu âm, đen nếu dương
                    if (value < 0)
                        return value.ToString("F2"); // Sẽ có dấu -
                    else
                        return value.ToString("F2");
                }
            }
            catch { }
            return odds;
        }

        private string FormatHandicap(double handicap)
        {
            try
            {
                if (Math.Abs(handicap) < 0.01) return "0.0";
                return handicap.ToString("F1");
            }
            catch { }
            return handicap.ToString();
        }

        private string FormatLine(double line)
        {
            try
            {
                if (Math.Abs(line) < 0.01) return "0.0";
                
                // Format line như trong hình: 4-4.5, 2-2.5, 3-3.5
                double lower = Math.Floor(line * 2) / 2;
                double upper = Math.Ceiling(line * 2) / 2;
                
                if (Math.Abs(lower - upper) < 0.01)
                {
                    return line.ToString("F1");
                }
                else
                {
                    return $"{lower:F1}-{upper:F1}";
                }
            }
            catch { }
            return line.ToString();
        }

        private string ParseStatus(string statusCode)
        {
            return statusCode switch
            {
                "10" => "Hòa",
                "6" => "Live",
                "8" => "Live",
                "2" => "",
                _ => ""
            };
        }

        private string ParseDateTime(string dateStr)
        {
            try
            {
                // Format: "12/18/2025 01:00"
                if (DateTime.TryParse(dateStr, out DateTime dateTime))
                {
                    return dateTime.ToString("HH:mm");
                }
            }
            catch { }
            return "";
        }

        private string DecodeString(string encoded)
        {
            try
            {
                // Decode hex escapes like \xC3
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
