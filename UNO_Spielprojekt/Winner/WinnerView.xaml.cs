﻿using System.Windows;

namespace UNO_Spielprojekt.Winner;

public partial class WinnerView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(WinnerViewModel), typeof(WinnerView), new PropertyMetadata(default(WinnerViewModel)));

    public WinnerView()
    {
        InitializeComponent();
    }

    public WinnerViewModel ViewModel
    {
        get => (WinnerViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}