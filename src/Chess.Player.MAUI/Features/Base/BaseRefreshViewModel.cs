using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net;

namespace Chess.Player.MAUI.Features;

public abstract partial class BaseRefreshViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand), nameof(RefreshCommand), nameof(LoadCommand))]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isSuccessfullyLoaded;

    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    protected bool CanRefresh => !IsLoading;

    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private Task StartAsync(CancellationToken cancellationToken)
    {
        UseCache = true;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private Task RefreshAsync(CancellationToken cancellationToken)
    {
        UseCache = false;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanRefresh), IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            Progress = 0;

            await LoadDataAsync(cancellationToken);

            Progress = 100;

            IsSuccessfullyLoaded = true;
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
            UseCache = false;
            IsLoading = false;
        }
    }

    protected abstract Task LoadDataAsync(CancellationToken cancellationToken);
}