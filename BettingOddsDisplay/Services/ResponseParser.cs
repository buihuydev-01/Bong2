using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BettingOddsDisplay.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BettingOddsDisplay.Services
{
    public class ResponseParser
    {
        public BettingData ParseResponse(string response)
        {
            var data = new BettingData();
            
            try
            {
                Console.WriteLine($"[Parser] Response length: {response.Length}");
                Console.WriteLine($"[Parser] First 200 chars: {response.Substring(0, Math.Min(200, response.Length))}");
                
                // Extract JSON array from JavaScript function call
                // Pattern: $M('odds-display').onUpdate(2,[...]);
                var regexMatch = Regex.Match(response, @"\$M\('odds-display'\)\.onUpdate\(2,(\[.+\])\)", RegexOptions.Singleline);
                
                if (!regexMatch.Success)
                {
                    Console.WriteLine("[Parser] ERROR: Regex did not match!");
                    Console.WriteLine("[Parser] Looking for pattern: $M('odds-display').onUpdate(2,[...])");
                    return data;
                }

                Console.WriteLine("[Parser] Regex matched successfully");
                var jsonString = regexMatch.Groups[1].Value;
                Console.WriteLine($"[Parser] JSON string length: {jsonString.Length}");
                Console.WriteLine($"[Parser] JSON first 200 chars: {jsonString.Substring(0, Math.Min(200, jsonString.Length))}");
                
                var rootArray = JArray.Parse(jsonString);
                Console.WriteLine($"[Parser] Root array parsed, count: {rootArray.Count}");

                if (rootArray.Count < 7)
                {
                    Console.WriteLine($"[Parser] ERROR: Root array too short, count={rootArray.Count}, expected >= 7");
                    return data;
                }

                Console.WriteLine("[Parser] Parsing leagues at index 3...");
                // Parse leagues [index 3]
                var leaguesArray = rootArray[3] as JArray;
                if (leaguesArray != null && leaguesArray.Count > 0)
                {
                    Console.WriteLine($"[Parser] Leagues array found, count: {leaguesArray.Count}");
                    var leaguesList = leaguesArray[0] as JArray;
                    if (leaguesList != null)
                    {
                        Console.WriteLine($"[Parser] Leagues list found, count: {leaguesList.Count}");
                        foreach (var league in leaguesList)
                        {
                            var leagueArray = league as JArray;
                            if (leagueArray != null && leagueArray.Count >= 4)
                            {
                                data.Leagues.Add(new League
                                {
                                    LeagueId = leagueArray[0].Value<int>(),
                                    Name = leagueArray[1].Value<string>() ?? "",
                                    SubLeague = leagueArray[2].Value<string>() ?? "",
                                    MatchInfo = leagueArray[3].Value<string>() ?? ""
                                });
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("[Parser] WARNING: No leagues array found at index 3");
                }

                Console.WriteLine($"[Parser] Total leagues parsed: {data.Leagues.Count}");
                Console.WriteLine("[Parser] Parsing matches at index 4...");
                
                // Parse matches [index 4]
                var matchesArray = rootArray[4] as JArray;
                if (matchesArray != null && matchesArray.Count > 0)
                {
                    Console.WriteLine($"[Parser] Matches array found, count: {matchesArray.Count}");
                    var matchesList = matchesArray[0] as JArray;
                    if (matchesList != null)
                    {
                        Console.WriteLine($"[Parser] Matches list found, count: {matchesList.Count}");
                        foreach (var matchToken in matchesList)
                        {
                            var matchArray = matchToken as JArray;
                            if (matchArray != null && matchArray.Count >= 12)
                            {
                                var matchObj = new Models.Match
                                {
                                    MatchId = matchArray[0].Value<long>(),
                                    SportType = matchArray[1].Value<int>(),
                                    LeagueId = matchArray[2].Value<int>(),
                                    HomeTeam = matchArray[3].Value<string>() ?? "",
                                    AwayTeam = matchArray[4].Value<string>() ?? "",
                                    MatchCode = matchArray[5].Value<string>() ?? "",
                                    Status = matchArray[6].Value<int>(),
                                    MatchTime = ParseDateTime(matchArray[7].Value<string>() ?? ""),
                                    LiveStatus = matchArray[8].Value<int>(),
                                    StreamType = matchArray[9].Value<string>() ?? "",
                                    Flags = matchArray[10].Value<int>()
                                };
                                
                                data.Matches.Add(matchObj);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("[Parser] WARNING: No matches array found at index 4");
                }

                Console.WriteLine($"[Parser] Total matches parsed: {data.Matches.Count}");
                Console.WriteLine("[Parser] Parsing markets at index 5...");
                
                // Parse markets [index 5]
                var marketsArray = rootArray[5] as JArray;
                if (marketsArray != null && marketsArray.Count > 0)
                {
                    var marketsList = marketsArray[0] as JArray;
                    if (marketsList != null)
                    {
                        foreach (var marketToken in marketsList)
                        {
                            var marketArray = marketToken as JArray;
                            if (marketArray != null && marketArray.Count >= 2)
                            {
                                long marketId = marketArray[0].Value<long>();
                                long matchId = marketArray[1].Value<long>();
                                
                                var matchObj = data.Matches.FirstOrDefault(m => m.MatchId == matchId);
                                if (matchObj != null)
                                {
                                    matchObj.MarketId = marketId;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("[Parser] WARNING: No markets array found at index 5");
                }

                Console.WriteLine("[Parser] Parsing odds at index 6...");
                
                // Parse odds [index 6]
                var oddsArray = rootArray[6] as JArray;
                if (oddsArray != null && oddsArray.Count > 0)
                {
                    var oddsList = oddsArray[0] as JArray;
                    if (oddsList != null)
                    {
                        foreach (var oddsToken in oddsList)
                        {
                            var oddsArr = oddsToken as JArray;
                            if (oddsArr != null && oddsArr.Count >= 3)
                            {
                                long oddsId = oddsArr[0].Value<long>();
                                var oddsInfo = oddsArr[1] as JArray;
                                var oddsValues = oddsArr[2] as JArray;
                                
                                if (oddsInfo != null && oddsInfo.Count >= 4 && oddsValues != null && oddsValues.Count >= 2)
                                {
                                    long marketId = oddsInfo[0].Value<long>();
                                    int betType = oddsInfo[1].Value<int>();
                                    int priority = oddsInfo[3].Value<int>();
                                    double handicap = 0;
                                    
                                    // Get handicap if available
                                    if (oddsInfo.Count >= 5 && oddsInfo[4] != null && oddsInfo[4].Type != JTokenType.Null)
                                    {
                                        handicap = oddsInfo[4].Value<double>();
                                    }
                                    
                                    var matchObj = data.Matches.FirstOrDefault(m => m.MarketId == marketId);
                                    if (matchObj != null)
                                    {
                                        var oddsGroup = matchObj.OddsGroups.FirstOrDefault(g => g.BetType == betType);
                                        if (oddsGroup == null)
                                        {
                                            oddsGroup = new OddsGroup { BetType = betType };
                                            matchObj.OddsGroups.Add(oddsGroup);
                                        }
                                        
                                        var oddsLine = new OddsLine
                                        {
                                            OddsId = oddsId,
                                            Handicap = handicap,
                                            Priority = priority
                                        };
                                        
                                        // Parse odds values
                                        if (oddsValues.Count == 2)
                                        {
                                            // Home/Away odds
                                            oddsLine.HomeOdds = oddsValues[0].Value<double>();
                                            oddsLine.AwayOdds = oddsValues[1].Value<double>();
                                        }
                                        else if (oddsValues.Count == 3)
                                        {
                                            // 1X2 odds
                                            oddsLine.HomeOdds = oddsValues[0].Value<double>();
                                            oddsLine.DrawOdds = oddsValues[1].Value<double>();
                                            oddsLine.AwayOdds = oddsValues[2].Value<double>();
                                        }
                                        
                                        oddsGroup.Lines.Add(oddsLine);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("[Parser] WARNING: No odds array found at index 6");
                }

                Console.WriteLine("[Parser] Sorting odds lines...");
                
                // Sort odds lines by priority
                foreach (var match in data.Matches)
                {
                    foreach (var group in match.OddsGroups)
                    {
                        group.Lines = group.Lines.OrderBy(l => l.Priority).ToList();
                    }
                }

                data.UpdateTime = DateTime.Now.ToString("HH:mm:ss");
                
                Console.WriteLine($"[Parser] ✅ Parse completed successfully!");
                Console.WriteLine($"[Parser] Final result: {data.Leagues.Count} leagues, {data.Matches.Count} matches");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Parser] ❌ JSON Parse error: {ex.Message}");
                Console.WriteLine($"[Parser] Stack trace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Parser] ❌ Parse error: {ex.Message}");
                Console.WriteLine($"[Parser] Stack trace: {ex.StackTrace}");
            }

            return data;
        }

        private DateTime ParseDateTime(string dateStr)
        {
            try
            {
                // Format: "12/18/2025 11:00"
                if (DateTime.TryParse(dateStr, out DateTime result))
                {
                    return result;
                }
            }
            catch { }
            
            return DateTime.Now;
        }
    }
}
