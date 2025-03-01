﻿using System.Windows;
using System.Windows.Controls;

namespace UNO_Spielprojekt.AddPlayer;

public partial class AddPlayerView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(AddPlayerViewModel), typeof(AddPlayerView),
        new PropertyMetadata(default(AddPlayerViewModel)));

    public PlayerData PlayerData;

    public AddPlayerView()
    {
        PlayerData = new PlayerData();
        InitializeComponent();
    }

    private void PlayerNameChanged(object sender, TextChangedEventArgs e)
    {
        UpdateWeiterButtonVisibility();
    }

    private void AddPlayerClicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.PlayerNames.Count < 5)
        {
            ViewModel.PlayerNames.Add(new NewPlayerViewModel());
        }

        UpdateWeiterButtonVisibility();
    }

    private void RemovePlayerClicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.PlayerNames.Count > 0)
        {
            ViewModel.PlayerNames.RemoveAt(ViewModel.PlayerNames.Count - 1);
        }

        UpdateWeiterButtonVisibility();
    }

    private void UpdateWeiterButtonVisibility()
    {
        if (ViewModel.PlayerNames.Count <= 1)
        {
            ContinueButton.Visibility = Visibility.Hidden;
        }
        else
        {
            foreach (var t in ViewModel.PlayerNames)
            {
                var allFieldsFilled = !string.IsNullOrWhiteSpace(t.Name);
                ContinueButton.Visibility = allFieldsFilled ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }

    public AddPlayerViewModel ViewModel
    {
        get => (AddPlayerViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}