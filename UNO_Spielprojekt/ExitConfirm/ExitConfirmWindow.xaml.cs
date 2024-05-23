﻿using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Window;
using System.Windows;

namespace UNO_Spielprojekt.ExitConfirm;

public partial class ExitConfirmWindow
{
    public static readonly DependencyProperty GameViewModelProperty = DependencyProperty.Register(
        nameof(GameViewModel), typeof(GameViewModel), typeof(ExitConfirmWindow),
        new PropertyMetadata(default(GameViewModel)));

    public GameViewModel GameViewModel
    {
        get => (GameViewModel)GetValue(GameViewModelProperty);
        set => SetValue(GameViewModelProperty, value);
    }


    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(ExitConfirmViewModel), typeof(ExitConfirmWindow),
        new PropertyMetadata(default(ExitConfirmViewModel)));

    public ExitConfirmViewModel ViewModel
    {
        get => (ExitConfirmViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    private readonly MainViewModel MainViewModel;

    public ExitConfirmWindow(MainViewModel mainViewModel)
    {
        MainViewModel = mainViewModel;
        ViewModel = new ExitConfirmViewModel(MainViewModel);
        InitializeComponent();
    }

    private void CloseExitConfirmWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ConfirmButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
        ViewModel.ConfirmButtonCommandMethod();
    }
}