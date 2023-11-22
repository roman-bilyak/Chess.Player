﻿using Chess.Player.Data;

namespace Chess.Player.MAUI.Features.Favorite;

public interface IPlayerFavoriteService
{
    Task AddAsync(string name, CancellationToken cancellationToken);

    Task RemoveAsync(string name, CancellationToken cancellationToken);

    Task<bool> ToggleAsync(string name, CancellationToken cancellationToken);

    Task<bool> ContainsAsync(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<PlayerFullInfo>> GetAllAsync(bool useCache, CancellationToken cancellationToken);

    Task ClearAsync(CancellationToken cancellationToken);
}