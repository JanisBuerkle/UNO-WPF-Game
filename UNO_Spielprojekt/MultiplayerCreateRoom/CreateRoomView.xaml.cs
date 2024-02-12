using System.Windows;
using System.Windows.Controls;

namespace UNO_Spielprojekt.MultiplayerCreateRoom;

public partial class CreateRoomView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(CreateRoomViewModel), typeof(CreateRoomView),
        new PropertyMetadata(default(CreateRoomViewModel)));

    public CreateRoomViewModel ViewModel
    {
        get => (CreateRoomViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public CreateRoomView()
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

        if (ViewModel.IsChecked && PasswortBox.Text.Contains(" ") || ViewModel.IsChecked && PasswortBox.Text == "")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }
    }

    private void CheckBoxClicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.IsChecked && PasswortBox.Text.Contains(" ") || ViewModel.IsChecked && PasswortBox.Text == "")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }
    }

    private void PasswortBoxChanged(object sender, TextChangedEventArgs e)
    {
        if (ViewModel.IsChecked && PasswortBox.Text.Contains(" ") || ViewModel.IsChecked && PasswortBox.Text == "")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }
    }
}