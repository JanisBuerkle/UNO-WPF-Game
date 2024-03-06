using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UNO_Spielprojekt.GamePage;

namespace UNO_Spielprojekt.MultiplayerGamePage;

public partial class MPGamePageView : UserControl
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(MPGamePageViewModel), typeof(MPGamePageView), new PropertyMetadata(default(MPGamePageViewModel)));

    public MPGamePageViewModel ViewModel
    {
        get { return (MPGamePageViewModel)GetValue(ViewModelProperty); }
        set { SetValue(ViewModelProperty, value); }
    }
    
    public MPGamePageView()
    {
        InitializeComponent();
    }
    
    private void CardButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is CardViewModel card)
        {
            int selectedIndex = ViewModel.CurrentHand.IndexOf(card);
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
}