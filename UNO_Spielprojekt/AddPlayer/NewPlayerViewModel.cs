namespace UNO_Spielprojekt.AddPlayer;

public class NewPlayerViewModel : ViewModelBase
{
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }
}