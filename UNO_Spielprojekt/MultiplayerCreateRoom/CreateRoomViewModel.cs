namespace UNO_Spielprojekt.MultiplayerCreateRoom;

public class CreateRoomViewModel : ViewModelBase
{
    private bool _isChecked;

    public bool IsChecked
    {
        get { return _isChecked; }
        set
        {
            _isChecked = value;
            OnPropertyChanged("IsChecked");
        }
    }
    
    public CreateRoomViewModel()
    {
        
    }
}