﻿using Chess.Player.Data;

namespace Chess.Player.MAUI.Services;

public interface IPlayerHistoryService
{
    Task AddAsync(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<PlayerFullInfo>> GetAllAsync(bool useCache, CancellationToken cancellationToken);

    Task ClearAsync(CancellationToken cancellationToken);
}