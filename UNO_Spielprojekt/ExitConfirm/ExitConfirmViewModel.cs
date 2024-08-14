using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.ExitConfirm;

public class ExitConfirmViewModel : ViewModelBase
{
    private readonly MainViewModel mainViewModel;

    public ExitConfirmViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
    }

    public void ConfirmButtonCommandMethod()
    {
        mainViewModel.GoToMainMenu();
    }
}