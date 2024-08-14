using System.Windows;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.ExitConfirm;

public partial class ExitConfirmWindow
{
    public static readonly DependencyProperty GameViewModelProperty = DependencyProperty.Register(
        nameof(GameViewModel), typeof(GameViewModel), typeof(ExitConfirmWindow),
        new PropertyMetadata(default(GameViewModel)));


    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(ExitConfirmViewModel), typeof(ExitConfirmWindow),
        new PropertyMetadata(default(ExitConfirmViewModel)));

    private readonly MainViewModel mainViewModel;

    public ExitConfirmWindow(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
        ViewModel = new ExitConfirmViewModel(this.mainViewModel);
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

    public GameViewModel GameViewModel
    {
        get => (GameViewModel)GetValue(GameViewModelProperty);
        set => SetValue(GameViewModelProperty, value);
    }

    public ExitConfirmViewModel ViewModel
    {
        get => (ExitConfirmViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}