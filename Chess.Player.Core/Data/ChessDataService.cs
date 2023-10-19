using System.Globalization;

namespace Chess.Player.Data
{
    public class ChessDataService : IChessDataService
    {
        private readonly IChessDataFetcher _dataFetcher;
        private readonly ICacheManager _cacheManager;

        public event SearchProgressEventHandler? ProgressChanged;

        public ChessDataService(IChessDataFetcher dataFetcher, ICacheManager cacheManager) 
        {
            ArgumentNullException.ThrowIfNull(dataFetcher);
            ArgumentNullException.ThrowIfNull(cacheManager);

            _dataFetcher = dataFetcher;
            _cacheManager = cacheManager;
        }

        public async Task<SearchResult> SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken)
        {
            OnProgressChanged(1);

            SearchResult result = new();

            List<PlayerTournament> playerTournaments = new();
            foreach (SearchCriteria searchCriteria in searchCriterias)
            {
                string name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(searchCriteria.Name);
                string[] nameParts = name.Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);

                string? lastName = nameParts.FirstOrDefault();
                string? firstName = string.Join(" ", nameParts.Skip(1));

                name = string.Join(" ", lastName, firstName);
                result.Names.Add(name);

                if (lastName is null)
                {
                    continue;
                }

                List<PlayerTournament>? tournaments = await _dataFetcher.GetPlayerTournamentsAsync(lastName, firstName, cancellationToken);
                if (tournaments is not null)
                {
                    playerTournaments.AddRange(tournaments);            
                }
            }

            int i = 0;
            List<PlayerTournamentInfo> playerTournamentInfos = new();
            foreach (PlayerTournament playerTournament in playerTournaments)
            {
                TournamentInfo? tournamentInfo = await _cacheManager.GetOrAddAsync(nameof(TournamentInfo), $"{playerTournament.TournamentId}",
                    () => _dataFetcher.GetTournamentInfoAsync(playerTournament.TournamentId, cancellationToken)
                );

                PlayerInfo? playerInfo = await _cacheManager.GetOrAddAsync(nameof(PlayerInfo), $"{playerTournament.TournamentId}_{playerTournament.PlayerStartingRank}",
                    () => _dataFetcher.GetPlayerInfoAsync(playerTournament.TournamentId, playerTournament.PlayerStartingRank, cancellationToken)
                );

                if (tournamentInfo is not null && playerInfo is not null)
                {
                    playerTournamentInfos.Add(new PlayerTournamentInfo(tournamentInfo, playerInfo));
                }

                int progressPercentage = i++ * 100 / playerTournaments.Count;
                OnProgressChanged(progressPercentage);
            }

            playerTournamentInfos = playerTournamentInfos.OrderByDescending(x => x.Tournament.EndDate).ToList();
            result.AddRange(playerTournamentInfos);

            OnProgressChanged(100);
            return result;
        }

        protected virtual void OnProgressChanged(int progressPercentage)
        {
            ProgressChanged?.Invoke(this, new SearchProgressEventArgs(progressPercentage));
        }
    }
}