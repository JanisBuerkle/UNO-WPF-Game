using UNO.Contract;

namespace UNO_Server.Models;

public class StartModel
{
    public readonly List<Card> WildCards = new List<Card>()
    {
        new Card
            { Color = "Red", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/red.png" },
        new Card
            { Color = "Blue", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/blue.png" },
        new Card
            { Color = "Yellow", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/yellow.png" },
        new Card
            { Color = "Green", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/wild/green.png" },
    };

    public readonly List<Card> Draw4Cards = new List<Card>()
    {
        new Card
            { Color = "Red", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/red.png" },
        new Card
            { Color = "Blue", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/blue.png" },
        new Card
            { Color = "Yellow", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/yellow.png" },
        new Card
            { Color = "Green", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/green.png" },
    };

    public readonly List<Card> cards = new List<Card>()
    {
        new Card
            { Color = "Blue", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Blue.png" },
        new Card
            { Color = "Blue", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Blue.png" },
        new Card
            { Color = "Blue", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Blue.png" },
        new Card
            { Color = "Blue", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Blue.png" },
        new Card
            { Color = "Blue", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Blue.png" },
        new Card
            { Color = "Blue", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Blue.png" },
        new Card
            { Color = "Blue", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Blue.png" },
        new Card
            { Color = "Blue", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Blue.png" },
        new Card
            { Color = "Blue", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Blue.png" },
        new Card
            { Color = "Blue", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Blue.png" },
        new Card
            { Color = "Blue", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Blue.png" },
        new Card
            { Color = "Blue", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Blue.png" },
        new Card
            { Color = "Blue", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Blue.png" },
        new Card
            { Color = "Blue", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Blue.png" },
        new Card
            { Color = "Blue", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Blue.png" },
        new Card
            { Color = "Blue", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Blue.png" },
        new Card
            { Color = "Blue", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Blue.png" },
        new Card
            { Color = "Blue", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Blue.png" },
        new Card
        {
            Color = "Blue", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Blue.png"
        },
        new Card
        {
            Color = "Blue", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Blue.png"
        },
        new Card
            { Color = "Blue", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Blue.png" },
        new Card
            { Color = "Blue", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Blue.png" },
        new Card
            { Color = "Blue", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Blue.png" },
        new Card
            { Color = "Blue", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Blue.png" },

        new Card
            { Color = "Green", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Green.png" },
        new Card
            { Color = "Green", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Green.png" },
        new Card
            { Color = "Green", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Green.png" },
        new Card
            { Color = "Green", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Green.png" },
        new Card
            { Color = "Green", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Green.png" },
        new Card
            { Color = "Green", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Green.png" },
        new Card
            { Color = "Green", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Green.png" },
        new Card
            { Color = "Green", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Green.png" },
        new Card
            { Color = "Green", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Green.png" },
        new Card
            { Color = "Green", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Green.png" },
        new Card
            { Color = "Green", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Green.png" },
        new Card
            { Color = "Green", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Green.png" },
        new Card
            { Color = "Green", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Green.png" },
        new Card
            { Color = "Green", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Green.png" },
        new Card
            { Color = "Green", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Green.png" },
        new Card
            { Color = "Green", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Green.png" },
        new Card
            { Color = "Green", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Green.png" },
        new Card
            { Color = "Green", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Green.png" },
        new Card
        {
            Color = "Green", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Green.png"
        },
        new Card
        {
            Color = "Green", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Green.png"
        },
        new Card
            { Color = "Green", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Green.png" },
        new Card
            { Color = "Green", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Green.png" },
        new Card
            { Color = "Green", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Green.png" },
        new Card
            { Color = "Green", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Green.png" },

        new Card
            { Color = "Red", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Red.png" },
        new Card
            { Color = "Red", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Red.png" },
        new Card
            { Color = "Red", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Red.png" },
        new Card
            { Color = "Red", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Red.png" },
        new Card
            { Color = "Red", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Red.png" },
        new Card
            { Color = "Red", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Red.png" },
        new Card
            { Color = "Red", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Red.png" },
        new Card
            { Color = "Red", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Red.png" },
        new Card
            { Color = "Red", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Red.png" },
        new Card
            { Color = "Red", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Red.png" },
        new Card
            { Color = "Red", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Red.png" },
        new Card
            { Color = "Red", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Red.png" },
        new Card
            { Color = "Red", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Red.png" },
        new Card
            { Color = "Red", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Red.png" },
        new Card
            { Color = "Red", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Red.png" },
        new Card
            { Color = "Red", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Red.png" },
        new Card
            { Color = "Red", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Red.png" },
        new Card
            { Color = "Red", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Red.png" },
        new Card
            { Color = "Red", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Red.png" },
        new Card
            { Color = "Red", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Red.png" },
        new Card
            { Color = "Red", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Red.png" },
        new Card
            { Color = "Red", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Red.png" },
        new Card
            { Color = "Red", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Red.png" },
        new Card
            { Color = "Red", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Red.png" },

        new Card
            { Color = "Yellow", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "1", ImageUri = "pack://application:,,,/Assets/cards/1/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "3", ImageUri = "pack://application:,,,/Assets/cards/3/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "4", ImageUri = "pack://application:,,,/Assets/cards/4/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "5", ImageUri = "pack://application:,,,/Assets/cards/5/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "6", ImageUri = "pack://application:,,,/Assets/cards/6/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "7", ImageUri = "pack://application:,,,/Assets/cards/7/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "8", ImageUri = "pack://application:,,,/Assets/cards/8/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "9", ImageUri = "pack://application:,,,/Assets/cards/9/Yellow.png" },
        new Card
        {
            Color = "Yellow", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Yellow.png"
        },
        new Card
        {
            Color = "Yellow", Value = "Reverse", ImageUri = "pack://application:,,,/Assets/cards/reverse/Yellow.png"
        },
        new Card
            { Color = "Yellow", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "Skip", ImageUri = "pack://application:,,,/Assets/cards/skip/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Yellow.png" },
        new Card
            { Color = "Yellow", Value = "+2", ImageUri = "pack://application:,,,/Assets/cards/+2/Yellow.png" },

        new Card
            { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },
        new Card
            { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },
        new Card
            { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },
        new Card
            { Color = "Wild", Value = "Wild", ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png" },

        new Card
            { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" },
        new Card
            { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" },
        new Card
            { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" },
        new Card
            { Color = "Draw", Value = "+4", ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png" },
    };

    public StartModel(RoomContext context)
    {
    }

    private readonly Random _random = new Random();

    public async Task ShuffleDeck(Room roomItem)
    {
        var number = roomItem.Cards.Count;
        while (number > 1)
        {
            number--;
            var card = _random.Next(number + 1);
            (roomItem.Cards[card], roomItem.Cards[number]) = (roomItem.Cards[number], roomItem.Cards[card]);
        }
    }

    public async Task DealCards(Room roomItem)
    {
        foreach (var player in roomItem.Players)
        {
            for (int i = 0; i < 2; i++)
            {
                player.PlayerHand.Add(roomItem.Cards.First());
                roomItem.Cards.Remove(roomItem.Cards.First());
            }
        }
    }
}