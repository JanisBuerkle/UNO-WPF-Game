namespace UNO_Spielprojekt.AddPlayer;

public class NewPlayerViewModel : ViewModelBase
{
    private string name;

    public string Name
    {
        get => name;
        set
        {
            if (value == name)
            {
                return;
            }

            name = value;
            OnPropertyChanged();
        }
    }
}