using System.Windows;

namespace UNO_Spielprojekt.Scoreboard;

public partial class ScoreboardView
{
    public static readonly DependencyProperty GameDataProperty = DependencyProperty.Register(
        nameof(GameData), typeof(GameData), typeof(ScoreboardView), new PropertyMetadata(default(GameData)));

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(ScoreboardViewModel), typeof(ScoreboardView),
        new PropertyMetadata(default(ScoreboardViewModel)));

    public ScoreboardView()
    {
        InitializeComponent();
    }

    public GameData GameData
    {
        get => (GameData)GetValue(GameDataProperty);
        set => SetValue(GameDataProperty, value);
    }

    public ScoreboardViewModel ViewModel
    {
        get => (ScoreboardViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}