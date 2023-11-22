﻿using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Features;

internal class MAUINavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public MAUINavigationService
    (
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _serviceProvider = serviceProvider;
    }

    public async Task NavigateToPlayerAsync(string playerName)
    {
        await NavigateToAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = playerName;
        });
    }

    public async Task NavigateToTournamentAsync(int tournamentId, string? tournamentName)
    {
        await NavigateToAsync<TournamentFullPage, TournamentFullViewModel>(x =>
        {
            x.TournamentId = tournamentId;
            x.TournamentName = tournamentName;
        });
    }

    public async Task NavigateToPlayerTournamentAsync(int tournamentId, string? tournamentName, int playerNo, string? playerName)
    {
        await NavigateToAsync<PlayerTournamentFullPage, PlayerTournamentFullViewModel>(x =>
        {
            x.TournamentId = tournamentId;
            x.TournamentName = tournamentName;
            x.PlayerNo = playerNo;
            x.PlayerName = playerName;
        });
    }

    #region helper methods

    protected async Task NavigateToAsync<TPage, TViewModel>(Action<TViewModel> init)
        where TPage : Page
        where TViewModel : BaseViewModel
    {
        TPage page = _serviceProvider.GetRequiredService<TPage>();

        TViewModel viewModel = (TViewModel)page.BindingContext;
        if (init is not null && viewModel is not null)
        {
            init(viewModel);
        }

        await Shell.Current.Navigation.PushAsync(page);
    }

    #endregion
}