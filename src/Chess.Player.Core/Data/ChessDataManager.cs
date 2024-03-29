﻿using Chess.Player.Cache;

namespace Chess.Player.Data;

internal class ChessDataManager : IChessDataManager
{
    private readonly IChessDataFetcher _chessDataFetcher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICacheManager _cacheManager;

    public event ProgressEventHandler? ProgressChanged;

    public ChessDataManager
    (
        IChessDataFetcher chessDataFetcher,
        IDateTimeProvider dateTimeProvider,
        ICacheManager cacheManager
    ) 
    {
        ArgumentNullException.ThrowIfNull(chessDataFetcher);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataFetcher = chessDataFetcher;
        _dateTimeProvider = dateTimeProvider;
        _cacheManager = cacheManager;
    }

    public async Task<PlayerFullInfo> GetPlayerFullInfoAsync(SearchCriteria[] searchCriterias, bool useCache, CancellationToken cancellationToken)
    {
        OnProgressChanged(ProgressHelper.Start);

        PlayerFullInfo playerFullInfo = new();

        List<PlayerTournament> playerTournaments = new();
        foreach (SearchCriteria searchCriteria in searchCriterias)
        {
            string[] nameParts = searchCriteria.Name.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string lastName = nameParts.First();
            string? firstName = string.Join(" ", nameParts.Skip(1));
            playerFullInfo.Names.Add(new NameInfo(lastName, firstName));

            PlayerTournamentList playerTournamentList = await _cacheManager.GetOrAddAsync($"{lastName}_{firstName}",
                useCache,
                async () => await _chessDataFetcher.GetPlayerTournamentListAsync(lastName, firstName, cancellationToken),
                x => _dateTimeProvider.UtcNow.AddHours(1),
                cancellationToken);

            playerTournaments.AddRange(playerTournamentList.Items);
        }

        int index = 1;
        List<PlayerTournamentInfo> playerTournamentInfos = new();
        foreach (PlayerTournament playerTournament in playerTournaments)
        {
            PlayerTournamentInfo playerTournamentInfo = await GetPlayerTournamentInfoAsync(playerTournament.TournamentId, playerTournament.PlayerNo, useCache, cancellationToken);
            playerTournamentInfos.Add(playerTournamentInfo);

            double progressPercentage = ProgressHelper.GetProgress(index++, playerTournaments.Count);
            OnProgressChanged(progressPercentage);
        }

        playerTournamentInfos = playerTournamentInfos.OrderByDescending(x => x.Tournament.EndDate).ThenByDescending(x => x.Tournament.StartDate).ToList();
        playerFullInfo.Tournaments.AddRange(playerTournamentInfos);

        OnProgressChanged(ProgressHelper.Finish);
        return playerFullInfo;
    }

    public async Task<TournamentInfo> GetTournamentInfoAsync(int tournamentId, bool useCache, CancellationToken cancellationToken)
    {
        return await _cacheManager.GetOrAddAsync($"{tournamentId}",
            useCache,
            async () => await _chessDataFetcher.GetTournamentInfoAsync(tournamentId, cancellationToken),
            x => GetExpirationDate(x),
            cancellationToken);
    }

    public async Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, int playerNo, bool useCache, CancellationToken cancellationToken)
    {
        TournamentInfo tournamentInfo = await GetTournamentInfoAsync(tournamentId, useCache, cancellationToken);

        PlayerInfo playerInfo = await _cacheManager.GetOrAddAsync($"{tournamentId}_{playerNo}",
            useCache,
            async () => await _chessDataFetcher.GetPlayerInfoAsync(tournamentId, playerNo, cancellationToken),
            x => GetExpirationDate(tournamentInfo),
            cancellationToken);

        return new PlayerTournamentInfo(tournamentInfo, playerInfo);
    }

    #region helper methods

    protected virtual void OnProgressChanged(double percentage)
    {
        ProgressChanged?.Invoke(this, new ProgressEventArgs(percentage));
    }

    private DateTime? GetExpirationDate(TournamentInfo tournamentInfo)
    {
        DateTime utcNow = _dateTimeProvider.UtcNow;

        DateTime startDate = tournamentInfo.StartDate?.Date ?? DateTime.MinValue;
        DateTime endDate = tournamentInfo.EndDate?.Date ?? DateTime.MaxValue;

        if (utcNow.Date < startDate)
        {
            return utcNow.AddHours(1);
        }
        else if (utcNow.Date > endDate)
        {
            return null;
        }
        else
        {
            return utcNow.AddMinutes(1);
        }
    }

    #endregion
}