using System;
using System.IO;
using BettingOddsDisplay.Services;

namespace BettingOddsDisplay
{
    /// <summary>
    /// Test class để test parser offline với sample data
    /// Chạy: dotnet run --project BettingOddsDisplay.csproj TestOffline
    /// </summary>
    public class TestOffline
    {
        public static void TestParserWithSampleData()
        {
            Console.WriteLine("=== Testing Parser with Sample Data ===\n");

            var sampleFile = "sample_response.txt";
            
            if (!File.Exists(sampleFile))
            {
                Console.WriteLine($"ERROR: {sampleFile} not found!");
                Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
                return;
            }

            var response = File.ReadAllText(sampleFile);
            Console.WriteLine($"Loaded sample response: {response.Length} characters\n");

            var parser = new ResponseParser();
            var data = parser.ParseResponse(response);

            Console.WriteLine($"✅ Leagues parsed: {data.Leagues.Count}");
            foreach (var league in data.Leagues)
            {
                Console.WriteLine($"   - {league.Name}");
            }

            Console.WriteLine($"\n✅ Matches parsed: {data.Matches.Count}");
            foreach (var match in data.Matches)
            {
                Console.WriteLine($"   - {match.HomeTeam} vs {match.AwayTeam}");
                Console.WriteLine($"     Time: {match.TimeDisplay}");
                Console.WriteLine($"     Odds groups: {match.OddsGroups.Count}");
                
                foreach (var group in match.OddsGroups)
                {
                    Console.WriteLine($"       BetType {group.BetType}: {group.Lines.Count} lines");
                }
            }

            Console.WriteLine($"\n✅ Update time: {data.UpdateTime}");
            Console.WriteLine("\n=== Test completed successfully! ===");
        }
    }
}
