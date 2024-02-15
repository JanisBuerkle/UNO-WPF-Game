using System.Collections.ObjectModel;

namespace WpfApp2.GamePage;

public class PlayViewModel : ViewModelBase
{
    private ObservableCollection<CardViewModel> _cards = new();

    public ObservableCollection<CardViewModel> Cards
    {
        get => _cards;
        set
        {
            if (Equals(value, _cards)) return;
            _cards = value;
            OnPropertyChanged();
        }
    }
}