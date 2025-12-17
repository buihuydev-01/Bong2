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
                // Parse leagues - Format: [leagueId,'leagueName','','']
                var leaguesDict = new Dictionary<int, string>();
                var leaguePattern = @"\[(\d+),'([^']*)'(?:,'[^']*')?(?:,'[^']*')?\]";
                var leagueSection = ExtractSection(response, 0);
                if (!string.IsNullOrEmpty(leagueSection))
                {
                    var leagueMatches = Regex.Matches(leagueSection, leaguePattern);
                    foreach (Match leagueMatch in leagueMatches)
                    {
                        if (int.TryParse(leagueMatch.Groups[1].Value, out int leagueId))
                        {
                            var leagueName = DecodeString(leagueMatch.Groups[2].Value);
                            leaguesDict[leagueId] = leagueName;
                        }
                    }
                }

                // Parse matches - Format: [eventId,1,leagueId,'home','away','code',status,'datetime',...]
                var matchPattern = @"\[(\d+),1,(\d+),'([^']*)','([^']*)','[^']*',(\d+),'([^']*)',";
                var matchSection = ExtractSection(response, 1);
                if (!string.IsNullOrEmpty(matchSection))
                {
                    var matchMatches = Regex.Matches(matchSection, matchPattern);

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
                                Time = ParseDateTime(m.Groups[6].Value),
                                Status = ParseStatus(m.Groups[5].Value)
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
                }

                // Parse odds data from section 4 (actual odds values)
                ParseOddsData(response, matches);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing response: {ex.Message}");
            }

            return matches;
        }

        private string ExtractSection(string response, int sectionIndex)
        {
            try
            {
                // Response format: $M('odds-display').onUpdate(2,[[[section0]],[[section1]],[[section2]],[[section3]],...]);
                var mainPattern = @"\$M\('odds-display'\)\.onUpdate\(2,\[(.*?)\]\);";
                var mainMatch = Regex.Match(response, mainPattern, RegexOptions.Singleline);
                
                if (mainMatch.Success)
                {
                    var content = mainMatch.Groups[1].Value;
                    
                    // Split by top-level arrays
                    var sections = new List<string>();
                    int depth = 0;
                    int start = 0;
                    
                    for (int i = 0; i < content.Length; i++)
                    {
                        if (content[i] == '[')
                        {
                            if (depth == 0) start = i;
                            depth++;
                        }
                        else if (content[i] == ']')
                        {
                            depth--;
                            if (depth == 0)
                            {
                                sections.Add(content.Substring(start, i - start + 1));
                            }
                        }
                    }
                    
                    if (sectionIndex < sections.Count)
                    {
                        return sections[sectionIndex];
                    }
                }
            }
            catch { }
            return string.Empty;
        }

        private void ParseOddsData(string response, List<Match> matches)
        {
            try
            {
                // Parse odds from section 4: [oddsId,[matchStatsId,betType,subType,handicap,stake],[odds1,odds2,odds3]]
                var oddsSection = ExtractSection(response, 4);
                if (string.IsNullOrEmpty(oddsSection)) return;

                // Pattern: [number,[matchStatsId,betType,subType,handicap,stake],[oddsArray]]
                var oddsPattern = @"\[(\d+),\[(\d+),(\d+),(\d+),([^,\]]+),[^\]]*\],\[([^\]]+)\]\]";
                var oddsMatches = Regex.Matches(oddsSection, oddsPattern);

                // Create mapping from matchStatsId to eventId (from section 2)
                var statsToEventMap = new Dictionary<int, int>();
                var section2 = ExtractSection(response, 2);
                if (!string.IsNullOrEmpty(section2))
                {
                    var statsPattern = @"\[(\d+),(\d+),";
                    var statsMatches = Regex.Matches(section2, statsPattern);
                    foreach (Match sm in statsMatches)
                    {
                        if (int.TryParse(sm.Groups[1].Value, out int statsId) && 
                            int.TryParse(sm.Groups[2].Value, out int eventId))
                        {
                            statsToEventMap[statsId] = eventId;
                        }
                    }
                }

                foreach (Match om in oddsMatches)
                {
                    try
                    {
                        var matchStatsId = int.Parse(om.Groups[2].Value);
                        var betType = int.Parse(om.Groups[3].Value);
                        var subType = int.Parse(om.Groups[4].Value);
                        var handicapStr = om.Groups[5].Value;
                        var oddsStr = om.Groups[6].Value;
                        
                        // Get eventId from statsId
                        if (!statsToEventMap.TryGetValue(matchStatsId, out int eventId))
                            continue;
                            
                        var match = matches.FirstOrDefault(m => m.EventId == eventId);
                        if (match == null) continue;

                        var oddsValues = oddsStr.Split(',');
                        
                        // Parse handicap value
                        double.TryParse(handicapStr, out double handicap);

                        // betType: 1=FT HDP, 3=FT OU, 5=FT 1X2, 7=HT HDP, 9=HT OU, 8=HT 1X2, 12=Odd/Even
                        switch (betType)
                        {
                            case 1: // Full Time Handicap
                                if (oddsValues.Length >= 2)
                                {
                                    match.FTHDPLine = FormatHandicap(handicap);
                                    match.FTHDPHome = FormatOdds(oddsValues[0]);
                                    match.FTHDPLineAway = FormatHandicap(-handicap);
                                    match.FTHDPAway = FormatOdds(oddsValues[1]);
                                }
                                break;

                            case 3: // Full Time Over/Under
                                if (oddsValues.Length >= 2)
                                {
                                    match.FTOULine = FormatLine(handicap);
                                    match.FTOUOver = FormatOdds(oddsValues[0]);
                                    match.FTOULineUnder = FormatLine(handicap);
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

                            case 7: // Half Time Handicap
                                if (oddsValues.Length >= 2)
                                {
                                    match.HTHDPLine = FormatHandicap(handicap);
                                    match.HTHDPHome = FormatOdds(oddsValues[0]);
                                    match.HTHDPLineAway = FormatHandicap(-handicap);
                                    match.HTHDPAway = FormatOdds(oddsValues[1]);
                                }
                                break;

                            case 9: // Half Time Over/Under
                                if (oddsValues.Length >= 2)
                                {
                                    match.HTOULine = FormatLine(handicap);
                                    match.HTOUOver = FormatOdds(oddsValues[0]);
                                    match.HTOULineUnder = FormatLine(handicap);
                                    match.HTOUUnder = FormatOdds(oddsValues[1]);
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
                        Console.WriteLine($"Error parsing odds entry: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing odds data: {ex.Message}");
            }
        }

        private string FormatOdds(string odds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(odds)) return "";
                odds = odds.Trim();
                
                if (double.TryParse(odds, out double value))
                {
                    if (Math.Abs(value) < 0.01) return "0";
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
                
                if (handicap > 0)
                    return handicap.ToString("F1");
                else if (handicap < 0)
                    return handicap.ToString("F1");
                else
                    return "0.0";
            }
            catch { }
            return handicap.ToString();
        }

        private string FormatLine(double line)
        {
            try
            {
                if (Math.Abs(line) < 0.01) return "0.0";
                return line.ToString("F1");
            }
            catch { }
            return line.ToString();
        }

        private string ParseStatus(string statusCode)
        {
            return statusCode switch
            {
                "10" => "Sắp diễn ra",
                "6" => "Live",
                "8" => "Live",
                "2" => "Kết thúc",
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
            return dateStr;
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
