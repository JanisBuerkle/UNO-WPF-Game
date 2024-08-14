using CommunityToolkit.Mvvm.Input;

namespace UNO_Spielprojekt.ChooseColor;

public class ChooseColorViewModel : ViewModelBase
{
    private int choosenColor;
    public RelayCommand ChooseRedCommand { get; }
    public RelayCommand ChooseBlueCommand { get; }
    public RelayCommand ChooseYellowCommand { get; }
    public RelayCommand ChooseGreenCommand { get; }

    public ChooseColorViewModel()
    {
        ChooseRedCommand = new RelayCommand(ChooseRedCommandMethod);
        ChooseBlueCommand = new RelayCommand(ChooseBlueCommandMethod);
        ChooseYellowCommand = new RelayCommand(ChooseYellowCommandMethod);
        ChooseGreenCommand = new RelayCommand(ChooseGreenCommandMethod);
    }

    private void ChooseRedCommandMethod()
    {
        ChoosenColor = (int)Color.Red;
    }

    private void ChooseBlueCommandMethod()
    {
        ChoosenColor = (int)Color.Blue;
    }

    private void ChooseYellowCommandMethod()
    {
        ChoosenColor = (int)Color.Yellow;
    }

    private void ChooseGreenCommandMethod()
    {
        ChoosenColor = (int)Color.Green;
    }

    public int ChoosenColor
    {
        get => choosenColor;
        private set
        {
            choosenColor = value;
            OnPropertyChanged();
        }
    }
}