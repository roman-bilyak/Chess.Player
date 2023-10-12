using System.Text;

namespace Chess.Player.Data
{
    internal class ConsoleOutputFormatter : IOutputFormatter
    {
        public void DisplayProgress(int progressPercentage)
        {
            Console.Write("\r");
            if (progressPercentage < 100)
            {
                Console.Write($"Loading ... {progressPercentage}%");
            }
        }

        public void DisplayResult(SearchResult searchResult)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine($"Last Name: {searchResult.LastName}");
            Console.WriteLine($"First Name: {searchResult.FirstName}");
            Console.WriteLine($"Title: {searchResult.Title}");
            Console.WriteLine($"FIDE ID: {searchResult.FideId}");
            Console.WriteLine($"Club/City: {searchResult.ClubCity}");
            Console.WriteLine($"Year Of Birth: {searchResult.YearOfBirth}");
            Console.WriteLine($"Years: {DateTime.Now.Year - searchResult.YearOfBirth}");

            int index = searchResult.Count;
            foreach (var group in searchResult.GroupBy(x => x.Tournament.EndDate?.Year))
            {
                Console.WriteLine();
                Console.WriteLine($"{group.Key} ({group.Key - searchResult.YearOfBirth} years) - {group.Count()} game(s)");

                foreach (PlayerTournamentInfo playerTournament in group)
                {
                    TournamentInfo tournament = playerTournament.Tournament;
                    PlayerInfo player = playerTournament.Player;

                    Console.WriteLine($"{index--,-3} {player.Title,-3} {tournament.EndDate:dd.MM.yy} {(tournament.IsTeamTournament ? null : player.Rank),4} {tournament.NumberOfPlayers,4} {player.Points,4} {tournament.NumberOfRounds,2}  {tournament.Name}");
                }
            }
        }
    }
}