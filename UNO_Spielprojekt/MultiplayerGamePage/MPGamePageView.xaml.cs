using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UNO.Contract;

namespace UNO_Spielprojekt.MultiplayerGamePage;

public partial class MpGamePageView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(MPGamePageViewModel), typeof(MpGamePageView),
        new PropertyMetadata(default(MPGamePageViewModel)));

    public MPGamePageViewModel ViewModel
    {
        get => (MPGamePageViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public MpGamePageView()
    {
        InitializeComponent();
    }

    private void CardButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is CardDTO card)
        {
            int selectedIndex = ViewModel.MultiplayerRoomsViewModel.Player.PlayerHand.IndexOf(card);
            ViewModel.SelectedCardIndex = selectedIndex;
            ViewModel.LegenCommandMethod();
        }
    }

    private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var scrollViewer = (ScrollViewer)sender;
        double scrollFactor = 1.0;
        scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta * scrollFactor);
        e.Handled = true;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        ItemsControl.Items.Refresh();
        ItemsControl.UpdateLayout();
    }
}