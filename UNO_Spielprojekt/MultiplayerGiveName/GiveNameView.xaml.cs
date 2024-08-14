using System.Windows;
using System.Windows.Controls;

namespace UNO_Spielprojekt.MultiplayerGiveName;

public partial class GiveNameView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(GiveNameViewModel), typeof(GiveNameView),
        new PropertyMetadata(default(GiveNameViewModel)));

    public GiveNameView()
    {
        InitializeComponent();
    }

    private void TextChanged(object sender, TextChangedEventArgs e)
    {
        if (TextBox.Text == "" || TextBox.Text == " ")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }
    }

    private void SaveName(object sender, RoutedEventArgs e)
    {
        ViewModel.MultiplayerRoomsViewModel.PlayerName = TextBox.Text;
    }

    public GiveNameViewModel ViewModel
    {
        get => (GiveNameViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}