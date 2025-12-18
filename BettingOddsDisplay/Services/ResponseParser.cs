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
                // Extract JSON array from JavaScript function call
                // Pattern: $M('odds-display').onUpdate(2,[...]);
                var regexMatch = Regex.Match(response, @"\$M\('odds-display'\)\.onUpdate\(2,(\[.+\])\);?", RegexOptions.Singleline);
                
                if (!regexMatch.Success)
                    return data;

                var jsonString = regexMatch.Groups[1].Value;
                var rootArray = JArray.Parse(jsonString);

                if (rootArray.Count < 7)
                    return data;

                // Parse leagues [index 3]
                var leaguesArray = rootArray[3] as JArray;
                if (leaguesArray != null && leaguesArray.Count > 0)
                {
                    var leaguesList = leaguesArray[0] as JArray;
                    if (leaguesList != null)
                    {
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

                // Parse matches [index 4]
                var matchesArray = rootArray[4] as JArray;
                if (matchesArray != null && matchesArray.Count > 0)
                {
                    var matchesList = matchesArray[0] as JArray;
                    if (matchesList != null)
                    {
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

                // Sort odds lines by priority
                foreach (var match in data.Matches)
                {
                    foreach (var group in match.OddsGroups)
                    {
                        group.Lines = group.Lines.OrderBy(l => l.Priority).ToList();
                    }
                }

                data.UpdateTime = DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Parse error: {ex.Message}");
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
