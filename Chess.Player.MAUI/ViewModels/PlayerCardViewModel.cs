﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerCardViewModel : BaseViewModel
{
    public string Name => Names.FirstOrDefault()?.FullName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name))]
    private ObservableCollection<NameViewModel> _names = new();

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