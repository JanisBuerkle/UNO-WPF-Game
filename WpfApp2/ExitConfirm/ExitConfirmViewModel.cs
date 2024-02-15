using WpfApp2.Window;

namespace WpfApp2.GamePage;

public class ExitConfirmViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    public ExitConfirmViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    public void ConfirmButtonCommandMethod()
    {
        _mainViewModel.GoToMainMenu();
    }
}