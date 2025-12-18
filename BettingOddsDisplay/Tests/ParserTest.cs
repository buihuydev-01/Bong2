using System;
using System.IO;
using BettingOddsDisplay.Services;

namespace BettingOddsDisplay.Tests
{
    /// <summary>
    /// Simple test class để test ResponseParser offline
    /// Compile: csc /reference:Newtonsoft.Json.dll ParserTest.cs
    /// Run: dotnet run
    /// </summary>
    public class ParserTest
    {
        public static void TestParser()
        {
            Console.WriteLine("=== Testing ResponseParser ===\n");

            // Sample response từ file
            var sampleResponse = @"$M('odds-display').onUpdate(2,[74748,1,1,[[[427465,'e-Football F24 Elite Club Friendly','','2 x 8 minutes'],[427468,'e-Football F24 International Friendly','','2 x 8 minutes']],[[9198111,1,427465,'e-Paris Saint Germain','e-Real Madrid','E3128251217219',8,'12/18/2025 11:00',0,'twitch',19,0,0,0],[9198112,1,427465,'e-Lille','e-Newcastle United','E3128251217220',8,'12/18/2025 11:00',0,'twitch',19,0,0,0],[9197896,1,427468,'e-England','e-Belgium','E3127251217171',8,'12/18/2025 11:00',0,'twitch',19,0,0,0],[9197897,1,427468,'e-Italy','e-Denmark','E3127251217172',8,'12/18/2025 11:00',0,'twitch',19,0,0,0]],[[113752374,9198111,0,0,0,19],[113752375,9198112,0,0,0,19],[113752020,9197896,0,0,0,19],[113752021,9197897,0,0,0,19]],,,[[5733788220,[113752374,1,0,500.00,-0.75],[0.67,0.77]],[5733788340,[113752374,2,0,200.00,0.00],[0.86,0.86]],[5733788350,[113752374,5,0,200.00,0],[3.15,3.97,1.72]]],,,'Bóng Đá',0],,,,0]);";

            var parser = new ResponseParser();
            var data = parser.ParseResponse(sampleResponse);

            Console.WriteLine($"✅ Parsed {data.Leagues.Count} leagues");
            foreach (var league in data.Leagues)
            {
                Console.WriteLine($"   - {league.Name} ({league.MatchInfo})");
            }

            Console.WriteLine($"\n✅ Parsed {data.Matches.Count} matches");
            foreach (var match in data.Matches)
            {
                Console.WriteLine($"   - {match.HomeTeam} vs {match.AwayTeam} at {match.TimeDisplay}");
                Console.WriteLine($"     Odds groups: {match.OddsGroups.Count}");
                
                foreach (var group in match.OddsGroups)
                {
                    Console.WriteLine($"     BetType {group.BetType}: {group.Lines.Count} lines");
                    foreach (var line in group.Lines.Take(2)) // Only show first 2 lines
                    {
                        if (group.BetType == 5) // 1X2
                        {
                            Console.WriteLine($"       {line.HomeOdds:F2} - {line.DrawOdds:F2} - {line.AwayOdds:F2}");
                        }
                        else
                        {
                            Console.WriteLine($"       {line.HandicapDisplay}: {line.HomeOdds:F2} / {line.AwayOdds:F2}");
                        }
                    }
                }
            }

            Console.WriteLine($"\n✅ Update time: {data.UpdateTime}");
            Console.WriteLine("\n=== Test completed successfully! ===");
        }

        public static void TestFromFile(string filePath)
        {
            Console.WriteLine($"=== Testing ResponseParser from file: {filePath} ===\n");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"❌ File not found: {filePath}");
                return;
            }

            var response = File.ReadAllText(filePath);
            var parser = new ResponseParser();
            var data = parser.ParseResponse(response);

            Console.WriteLine($"✅ Leagues: {data.Leagues.Count}");
            Console.WriteLine($"✅ Matches: {data.Matches.Count}");
            Console.WriteLine($"✅ Update time: {data.UpdateTime}");

            // Detailed output
            foreach (var match in data.Matches)
            {
                Console.WriteLine($"\n📊 {match.HomeTeam} vs {match.AwayTeam}");
                Console.WriteLine($"   Time: {match.TimeDisplay}, Status: {match.StatusDisplay}");
                Console.WriteLine($"   Market ID: {match.MarketId}");
                
                var totalOdds = match.OddsGroups.Sum(g => g.Lines.Count);
                Console.WriteLine($"   Total odds lines: {totalOdds}");
            }

            Console.WriteLine("\n=== Test completed! ===");
        }
    }
}
