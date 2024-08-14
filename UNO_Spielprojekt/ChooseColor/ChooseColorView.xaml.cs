using System.Windows;

namespace UNO_Spielprojekt.ChooseColor;

public partial class ChooseColorView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(ChooseColorViewModel), typeof(ChooseColorView),
        new PropertyMetadata(default(ChooseColorViewModel)));

    public ChooseColorView()
    {
        ViewModel = new ChooseColorViewModel();
        InitializeComponent();
    }

    public ChooseColorViewModel ViewModel
    {
        get => (ChooseColorViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}