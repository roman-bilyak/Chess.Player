using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels
{
    public partial class RecentPlayerViewModel: ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _lastName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _firstName;

        public string FullName => $"{LastName} {FirstName}";

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasClubCity))]
        private string _clubCity;

        public bool HasClubCity => !string.IsNullOrEmpty(ClubCity);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Years), nameof(HasYearOfBirth))]
        private int? _yearOfBirth;

        public bool HasYearOfBirth => YearOfBirth is not null;

        public int Years => DateTime.UtcNow.Year - YearOfBirth ?? 0;
    }
}