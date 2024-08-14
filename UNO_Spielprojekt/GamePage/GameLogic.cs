using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using tt.Tools.Logging;

namespace UNO_Spielprojekt.GamePage;

public class GameLogic
{
    public readonly List<Players> Players = new();
    private readonly ILogger logger;

    public readonly ObservableCollection<CardViewModel> Center = new();
    private readonly Random random = new();

    public readonly List<CardViewModel> WildCards = new()
    {
        new CardViewModel { Color = "Red", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/red.png" },
        new CardViewModel { Color = "Blue", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/blue.png" },
        new CardViewModel { Color = "Yellow", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/yellow.png" },
        new CardViewModel { Color = "Green", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/green.png" }
    };

    public readonly List<CardViewModel> Draw4Cards = new()
    {
        new CardViewModel { Color = "Red", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/red.png" },
        new CardViewModel { Color = "Blue", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/blue.png" },
        new CardViewModel { Color = "Yellow", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/yellow.png" },
        new CardViewModel { Color = "Green", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/green.png" }
    };

    public readonly List<CardViewModel> Cards = new()
    {
        new CardViewModel { Color = "Blue", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Blue.png" },
        new CardViewModel { Color = "Blue", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Blue.png" },

        new CardViewModel { Color = "Green", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Green.png" },
        new CardViewModel { Color = "Green", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Green.png" },
        new CardViewModel { Color = "Green", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Green.png" },
        new CardViewModel { Color = "Green", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Green.png" },
        new CardViewModel { Color = "Green", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Green.png" },
        new CardViewModel { Color = "Green", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Green.png" },
        new CardViewModel { Color = "Green", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Green.png" },
        new CardViewModel { Color = "Green", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Green.png" },
        new CardViewModel { Color = "Green", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Green.png" },
        new CardViewModel { Color = "Green", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Green.png" },
        new CardViewModel { Color = "Green", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Green.png" },
        new CardViewModel { Color = "Green", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Green.png" },
        new CardViewModel { Color = "Green", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Green.png" },
        new CardViewModel { Color = "Green", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Green.png" },
        new CardViewModel { Color = "Green", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Green.png" },
        new CardViewModel { Color = "Green", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Green.png" },
        new CardViewModel { Color = "Green", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Green.png" },
        new CardViewModel { Color = "Green", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Green.png" },
        new CardViewModel { Color = "Green", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Green.png" },
        new CardViewModel { Color = "Green", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Green.png" },
        new CardViewModel { Color = "Green", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Green.png" },
        new CardViewModel { Color = "Green", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Green.png" },
        new CardViewModel { Color = "Green", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Green.png" },
        new CardViewModel { Color = "Green", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Green.png" },

        new CardViewModel { Color = "Red", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Red.png" },
        new CardViewModel { Color = "Red", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Red.png" },
        new CardViewModel { Color = "Red", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Red.png" },
        new CardViewModel { Color = "Red", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Red.png" },
        new CardViewModel { Color = "Red", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Red.png" },
        new CardViewModel { Color = "Red", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Red.png" },
        new CardViewModel { Color = "Red", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Red.png" },
        new CardViewModel { Color = "Red", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Red.png" },
        new CardViewModel { Color = "Red", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Red.png" },
        new CardViewModel { Color = "Red", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Red.png" },
        new CardViewModel { Color = "Red", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Red.png" },
        new CardViewModel { Color = "Red", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Red.png" },
        new CardViewModel { Color = "Red", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Red.png" },
        new CardViewModel { Color = "Red", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Red.png" },
        new CardViewModel { Color = "Red", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Red.png" },
        new CardViewModel { Color = "Red", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Red.png" },
        new CardViewModel { Color = "Red", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Red.png" },
        new CardViewModel { Color = "Red", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Red.png" },
        new CardViewModel { Color = "Red", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Red.png" },
        new CardViewModel { Color = "Red", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Red.png" },
        new CardViewModel { Color = "Red", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Red.png" },
        new CardViewModel { Color = "Red", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Red.png" },
        new CardViewModel { Color = "Red", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Red.png" },
        new CardViewModel { Color = "Red", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Red.png" },

        new CardViewModel { Color = "Yellow", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Yellow.png" },
        new CardViewModel { Color = "Yellow", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Yellow.png" },

        new CardViewModel { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },
        new CardViewModel { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },
        new CardViewModel { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },
        new CardViewModel { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },

        new CardViewModel { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" },
        new CardViewModel { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" },
        new CardViewModel { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" },
        new CardViewModel { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" }
    };

    private PlayViewModel PlayViewModel { get; }

    public GameLogic(PlayViewModel playViewModel, ILogger loggerr)
    {
        logger = loggerr;
        PlayViewModel = playViewModel;
    }

    public int ChooseStartingPlayer()
    {
        logger.Info("StartingPlayer wurde ermittelt.");
        return random.Next(0, Players.Count);
    }

    public void ShuffleDeck()
    {
        logger.Info("Deck wurde gemischt.");
        var number = PlayViewModel.Cards.Count;
        while (number > 1)
        {
            number--;
            var card = random.Next(number + 1);
            (PlayViewModel.Cards[card], PlayViewModel.Cards[number]) =
                (PlayViewModel.Cards[number], PlayViewModel.Cards[card]);
        }
    }

    public void DealCards(int handSize)
    {
        foreach (var player in Players)
        {
            logger.Info($"{player.PlayerName} wurden {handSize} Karten ausgeteilt.");
            for (var i = 0; i < handSize; i++)
            {
                var card = PlayViewModel.Cards.First();
                PlayViewModel.Cards.RemoveAt(0);
                player.Hand.Add(card);
            }
        }
    }

    public void PlaceFirstCardInCenter()
    {
        logger.Info("Die Center Karte wurde ermittelt und gelegt.");
        var randomCard = random.Next(PlayViewModel.Cards.Count);
        var selectedCard = PlayViewModel.Cards[randomCard];
        PlayViewModel.Cards.RemoveAt(randomCard);
        Center.Add(selectedCard);
    }
}