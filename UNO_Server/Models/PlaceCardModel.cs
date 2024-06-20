namespace UNO_Server.Models;

public class PlaceCardModel
{
    private readonly Random _random = new Random();

    public void HandleSpecialCards(Room room, string[] splitted, int cardId, StartModel startModel)
    {
        foreach (var player in room.Players)
        {
            var playerCard = player.PlayerHand.FirstOrDefault(c => c.Id == cardId);
            if (playerCard != null)
            {
                string wildOrDraw = splitted[3];
                int number = Convert.ToInt32(splitted[4]);
                Card cardToPlace;
                switch (wildOrDraw)
                {
                    case "Draw":
                        cardToPlace = startModel.Draw4Cards[number];
                        break;
                    case "Wild":
                        cardToPlace = startModel.WildCards[number];
                        break;
                    default:
                        cardToPlace = null;
                        break;
                }

                if (cardToPlace != null)
                {
                    room.Center.Add(cardToPlace);
                    room.MiddleCard = cardToPlace;
                    room.SelectedCard = room.MiddleCard;
                    room.MiddleCardPic = room.MiddleCard.ImageUri;
                    player.PlayerHand.Remove(playerCard);
                }

                break;
            }
        }
    }

    public void HandleWildDrawCards(Room room, string value)
    {
        if (value == "+4")
        {
            for (int i = 0; i < 4; i++)
            {
                var rndCard = _random.Next(room.Cards.Count);
                var selectedCard = room.Cards[rndCard];
                room.Players[room.NextPlayer - 1].PlayerHand.Add(selectedCard);
            }
        }
    }

    public void HandleStandardCard(Room room, string color, string value, int cardId)
    {
        string path = $"pack://application:,,,/Assets/cards/{value}/{color}.png";
        var newCard = new Card() { Color = color, Value = value, ImageUri = path };
        room.Center.Add(newCard);
        room.MiddleCard = newCard;
        room.SelectedCard = newCard;
        room.MiddleCardPic = newCard.ImageUri;

        foreach (var player in room.Players)
        {
            var playerCard = player.PlayerHand.FirstOrDefault(c => c.Id == cardId);
            if (playerCard != null)
            {
                player.PlayerHand.Remove(playerCard);
                ApplyCardEffect(room, playerCard);
                break;
            }
        }
    }

    private void ApplyCardEffect(Room room, Card playerCard)
    {
        switch (playerCard.Value)
        {
            case "+2":
                for (int i = 0; i < 2; i++)
                {
                    var rndCard = _random.Next(room.Cards.Count);
                    var selectedCard = room.Cards[rndCard];
                    room.Players[room.NextPlayer - 1].PlayerHand.Add(selectedCard);
                }

                break;
            case "Skip":
            case "Reverse" when room.Players.Count == 2:
                room.IsSkip = true;
                break;
            case "Reverse":
                room.IsReverse = !room.IsReverse;
                break;
        }
    }
}