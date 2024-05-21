using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UNO_Spielprojekt.MultiplayerPassword;

public partial class PasswordView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(PasswordViewModel), typeof(PasswordView),
        new PropertyMetadata(default(PasswordViewModel)));

    public PasswordViewModel ViewModel
    {
        get => (PasswordViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public PasswordView()
    {
        InitializeComponent();
    }

    private void SaveInput(object sender, RoutedEventArgs e)
    {
        ViewModel.PasswordInput = TextBox.Text;
        if (ViewModel.MultiplayerRoomsViewModel.SelectedRoom2.Password == ViewModel.PasswordInput)
        {
            WrongPasswort.Foreground = Brushes.LimeGreen;
            WrongPasswort.Text = "Richtiges Passwort. Tritt bei...";
            ViewModel.PasswordCorrect();
        }
        else
        {
            WrongPasswort.Text = "Versuche es erneut. Falsches Passwort.";
        }
    }

    private void TextChanged(object sender, TextChangedEventArgs e)
    {
        WrongPasswort.Text = "";
    }
}