using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels
{
    internal partial class PlayerViewModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _lastName;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _firstName;

        public string FullName => $"{LastName} {FirstName}";

        [ObservableProperty]
        private string _data;

        [ObservableProperty]
        private bool _isLoading = false;

        [RelayCommand]
        private void Start()
        {
            IsLoading = true;
        }

        [RelayCommand(IncludeCancelCommand = true)]
        private async Task LoadAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(2 * 1000, cancellationToken);

                Data = Guid.NewGuid().ToString();
            }
            catch
            {
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
