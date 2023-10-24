﻿using Chess.Player.Data;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerViewModel : BaseViewModel, IDisposable
{
    private readonly IChessDataService _chessDataService;
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly IPlayerFavoriteService _playerFavoriteService;
    private readonly IPopupService _popupService;

    public string Name => Names.FirstOrDefault() ?? SearchCriterias.FirstOrDefault();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name))]
    private ObservableCollection<string> _searchCriterias = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name), nameof(HasNames))]
    private ObservableCollection<string> _names = new();

    public bool HasNames => Names.Any();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTitle))]
    private string _title;

    public bool HasTitle => !string.IsNullOrEmpty(Title);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasFideId))]
    private string _fideId;

    public bool HasFideId => !string.IsNullOrEmpty(FideId);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasClubCity))]
    private string _clubCity;

    public bool HasClubCity => !string.IsNullOrEmpty(ClubCity);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Years), nameof(HasYearOfBirth))]
    private int? _yearOfBirth;

    public bool HasYearOfBirth => YearOfBirth is not null;

    public int Years => DateTime.UtcNow.Year - YearOfBirth ?? 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTournamentYears))]
    private List<TournamentYearViewModel> _tournamentYears;

    public bool HasTournamentYears => TournamentYears?.Any() ?? false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Tournaments))]
    private TournamentYearViewModel _tournamentYear;

    private Dictionary<TournamentYearViewModel, List<PlayerTournamentViewModel>> _allTournaments;

    public List<PlayerTournamentViewModel> Tournaments => _allTournaments?.GetValueOrDefault(TournamentYear) ?? new List<PlayerTournamentViewModel>();

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    [ObservableProperty]
    private bool _forceRefresh;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ToggleFavoriteButtonName))]
    private bool _isFavorite;

    public string ToggleFavoriteButtonName => !IsFavorite ? "Add To Favorites" : "Remove From Favorites";

    public PlayerViewModel
    (
        IChessDataService chessDataService,
        IPlayerHistoryService historyService,
        IPlayerFavoriteService playerFavoriteService,
        IPopupService popupService
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(historyService);
        ArgumentNullException.ThrowIfNull(playerFavoriteService);
        ArgumentNullException.ThrowIfNull(popupService);

        _chessDataService = chessDataService;

        _playerHistoryService = historyService;
        _playerFavoriteService = playerFavoriteService;
        _popupService = popupService;

        _chessDataService.ProgressChanged += OnProgressChanged;
    }

    [RelayCommand]
    private void Start()
    {
        ForceRefresh = false;
        IsLoading = true;
    }

    [RelayCommand]
    private void Refresh()
    {
        ForceRefresh = true;
        IsLoading = true;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            SearchCriteria[] searchCriterias = SearchCriterias.Select(x => new SearchCriteria(x)).ToArray();
            SearchResult searchResult = await _chessDataService.SearchAsync(searchCriterias, ForceRefresh, cancellationToken);

            Names.Clear();
            foreach (string name in searchResult.Names)
            {
                Names.Add(name);
            }

            await _playerHistoryService.AddAsync(Name, cancellationToken);
            IsFavorite = await _playerFavoriteService.ContainsAsync(Name, cancellationToken);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(HasNames));

            Title = searchResult.Title;
            FideId = searchResult.FideId;
            ClubCity = searchResult.ClubCity;
            YearOfBirth = searchResult.YearOfBirth;

            int index = searchResult.Data.Count;
            _allTournaments = searchResult.Data.GroupBy(x => x.Tournament.EndDate?.Year)
                .ToDictionary(x => new TournamentYearViewModel
                {
                    Year = x.Key,
                    Years = x.Key - YearOfBirth ?? 0,
                    Count = x.Count()
                },
                x => x.Select(y => new PlayerTournamentViewModel
                {
                    TournamentNo = index--,
                    TournamentName = y.Tournament.Name,
                    TournamentLocation = y.Tournament.Location,
                    TournamentStartDate = y.Tournament.StartDate,
                    TournamentEndDate = y.Tournament.EndDate,
                    NumberOfPlayers = y.Tournament.NumberOfPlayers,
                    NumberOfRounds = y.Tournament.NumberOfRounds,
                    Title = y.Player.Title,
                    Rank = y.Player.Rank,
                    Points = y.Player.Points,
                }).ToList());

            TournamentYears = _allTournaments.Keys.ToList();
            TournamentYear = TournamentYears.FirstOrDefault(x => TournamentYear is null || x.Year == TournamentYear.Year);

            Error = null;
        }
        catch (OperationCanceledException)
        {

        }
        catch (WebException)
        {
            Error = "No internet connection.";
        }
        catch
        {
            Error = "Oops! Something went wrong.";
        }
        finally
        {
            IsLoading = false;
            ForceRefresh = true;
        }
    }

    [RelayCommand]
    private async Task AddSearchCriteriaAsync()
    {
        string searchCriteria = await _popupService.DisplayPromptAsync("Add Search Criteria", placeholder: "Example: Smith John");
        if (string.IsNullOrEmpty(searchCriteria?.Trim()))
        {
            return;
        }

        SearchCriterias.Add(searchCriteria);
        IsLoading = true;
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync(CancellationToken cancellationToken)
    {
        IsFavorite = await _playerFavoriteService.ToggleAsync(Name, cancellationToken);
    }

    private void OnProgressChanged(object sender, SearchProgressEventArgs e)
    {
        Progress = (double)e.ProgressPercentage / 100;
    }

    public void Dispose()
    {
        _chessDataService.ProgressChanged -= OnProgressChanged;
    }
}