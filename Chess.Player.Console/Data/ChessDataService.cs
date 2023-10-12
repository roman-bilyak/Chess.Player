namespace Chess.Player.Data
{
    internal class ChessDataService : IChessDataService
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

        public async Task<SearchResult> SearchAsync(params SearchCriteria[] searchCriterias)
        {
            OnProgressChanged(0);

            SearchResult result = new(searchCriterias.FirstOrDefault()?.LastName, searchCriterias.FirstOrDefault()?.FirstName);

            List<PlayerTournament> playerTournaments = new();
            foreach (SearchCriteria searchCriteria in searchCriterias)
            {
                List<PlayerTournament>? tournaments = await _dataFetcher.GetPlayerTournamentsAsync(searchCriteria.LastName, searchCriteria.FirstName);
                if (tournaments is not null)
                {
                    playerTournaments.AddRange(tournaments);            
                }
            }

            int i = 0;
            foreach (PlayerTournament playerTournament in playerTournaments)
            {
                TournamentInfo? tournamentInfo = await _cacheManager.GetOrAddAsync(nameof(TournamentInfo), $"{playerTournament.TournamentId}",
                    () => _dataFetcher.GetTournamentInfoAsync(playerTournament.TournamentId)
                );

                PlayerInfo? playerInfo = await _cacheManager.GetOrAddAsync(nameof(PlayerInfo), $"{playerTournament.TournamentId}_{playerTournament.PlayerStartingRank}",
                    () => _dataFetcher.GetPlayerInfoAsync(playerTournament.TournamentId, playerTournament.PlayerStartingRank)
                );

                if (tournamentInfo is not null && playerInfo is not null)
                {
                    result.Add(new PlayerTournamentInfo(tournamentInfo, playerInfo));
                }

                int progressPercentage = i++ * 100 / playerTournaments.Count;
                OnProgressChanged(progressPercentage);
            }

            OnProgressChanged(100);
            return result;
        }

        protected virtual void OnProgressChanged(int progressPercentage)
        {
            ProgressChanged?.Invoke(this, new SearchProgressEventArgs(progressPercentage));
        }
    }
}