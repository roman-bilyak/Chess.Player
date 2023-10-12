namespace Chess.Player.Data
{
    internal class SearchResult : SortedSet<PlayerTournamentInfo>
    {
        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? Title
        {
            get
            {
                return this.FirstOrDefault(x => x.Player.Title is not null)?.Player.Title;
            }
        }

        public string? FideId
        {
            get
            {
                return this.FirstOrDefault(x => x.Player.FideId is not null)?.Player.FideId;
            }
        }

        public string? ClubCity
        {
            get
            {
                return this.FirstOrDefault(x => x.Player.ClubCity is not null)?.Player.ClubCity;
            }
        }

        public int? YearOfBirth
        {
            get
            {
                return this.FirstOrDefault(x => x.Player.YearOfBirth is not null)?.Player.YearOfBirth;
            }
        }

        public SearchResult(string? lastName, string? firstName)
            : base(new SearchResultComparer())
        {
            LastName = lastName;
            FirstName = firstName;
        }
    }

    internal class SearchResultComparer : IComparer<PlayerTournamentInfo>
    {
        public int Compare(PlayerTournamentInfo? x, PlayerTournamentInfo? y)
        {
            if (x?.Tournament.EndDate is null && y?.Tournament.EndDate is null)
            {
                return 0;
            }
            else if (x?.Tournament.EndDate is null)
            {
                return 1;
            }
            else if (y?.Tournament.EndDate is null)
            {
                return -1;
            }
            else
            {
                return DateTime.Compare(y.Tournament.EndDate.Value, x.Tournament.EndDate.Value);
            }
        }
    }
}