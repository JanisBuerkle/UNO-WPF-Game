using Moq;
using tt.Tools.Logging;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Window;
using UNO.Contract;

namespace Unit_Testing;

public class MainViewModelTest
{
    private Mock<IRoomClient> roomClientMock;
    private MainViewModel MainViewModel { get; set; }

    [SetUp]
    public void Setup()
    {
        roomClientMock = new Mock<IRoomClient>();
        MainViewModel = new MainViewModel(roomClientMock.Object, new DoNothingLoggerFactory());
    }

    [Test]
    public void AddPlayerUnitTest()
    {
        // Arange
        MainViewModel.AddPlayerViewModel.PlayerNames.Add(new NewPlayerViewModel { Name = "Janis" });
        MainViewModel.AddPlayerViewModel.PlayerNames.Add(new NewPlayerViewModel { Name = "Vlas" });
            
        // Act
        MainViewModel.AddPlayerViewModel.WeiterButtonCommandMethod();

        // Assert
        Assert.That(MainViewModel.GameLogic.Players.Count, Is.EqualTo(2));
    }
    
    [Test]
    public void DealCardsUnitTest()
    {
        // Arange
        MainViewModel.GameLogic.Players.Add(new Players { PlayerName = "Janis" });
        MainViewModel.GameViewModel.InitializePlayersHands();
            
        // Act
        MainViewModel.GameLogic.DealCards(7);

        // Assert
        Assert.That(MainViewModel.GameLogic.Players[0].Hand.Count, Is.EqualTo(7));
    }
}