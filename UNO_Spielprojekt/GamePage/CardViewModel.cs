namespace UNO_Spielprojekt.GamePage;

public class CardViewModel : ViewModelBase
{
    private string value;

    private string color;

    private string imageUri;

    public string Value
    {
        get => value;
        set
        {
            if (value == this.value)
            {
                return;
            }

            this.value = value;
            OnPropertyChanged();
        }
    }

    public string Color
    {
        get => color;
        set
        {
            if (value == color)
            {
                return;
            }

            color = value;
            OnPropertyChanged();
        }
    }

    public string ImageUri
    {
        get => imageUri;
        set
        {
            if (Equals(value, imageUri))
            {
                return;
            }

            imageUri = value;
            OnPropertyChanged();
        }
    }
}