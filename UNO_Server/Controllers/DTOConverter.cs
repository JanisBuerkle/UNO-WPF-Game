using System.Collections.ObjectModel;
using UNO_Server.Models;
using UNO.Contract;

namespace UNO_Server.Controllers;

public class DTOConverter
{
    public Room DtoConverterMethod(RoomDTO roomDto)
    {
        var room = new Room();
        room.Id = roomDto.Id;
        room.RoomName = roomDto.RoomName;
        room.PlayButtonEnabled = roomDto.PlayButtonEnabled;
        room.PlayButtonContent = roomDto.PlayButtonContent;
        room.OnlineUsers = roomDto.OnlineUsers;

        foreach (var centerCard in roomDto.Center)
        {
            var newCenter = new Card()
            {
                Id = centerCard.Id, Color = centerCard.Color, Value = centerCard.Value, ImageUri = centerCard.ImageUri
            };
            room.Center.Add(newCenter);
        }

        var newMiddleCard = new Card()
        {
            Id = roomDto.MiddleCard.Id, Color = roomDto.MiddleCard.Color, Value = roomDto.MiddleCard.Value,
            ImageUri = roomDto.MiddleCard.ImageUri
        };
        room.MiddleCard = newMiddleCard;

        var newSelectedCard = new Card()
        {
            Id = roomDto.SelectedCard.Id, Color = roomDto.SelectedCard.Color, Value = roomDto.SelectedCard.Value,
            ImageUri = roomDto.SelectedCard.ImageUri
        };
        room.SelectedCard = newSelectedCard;

        room.MiddleCardPic = roomDto.MiddleCardPic;
        room.MaximalUsers = roomDto.MaximalUsers;
        room.PasswordSecured = roomDto.PasswordSecured;
        room.Password = roomDto.Password;
        room.StartingPlayer = roomDto.StartingPlayer;

        foreach (var player in roomDto.Players)
        {
            var newPlayerHand = new ObservableCollection<Card>();
            foreach (var playerHandCard in player.PlayerHand)
            {
                var newPlayerHandCard = new Card()
                {
                    Id = playerHandCard.Id, Color = playerHandCard.Color, Value = playerHandCard.Value,
                    ImageUri = playerHandCard.ImageUri
                };
                newPlayerHand.Add(newPlayerHandCard);
            }

            var newPlayer = new Player()
            {
                Id = player.Id, Name = player.Name, RoomId = player.RoomId, PlayerHand = newPlayerHand,
                IsLeader = player.IsLeader
            };
            room.Players.Add(newPlayer);
        }

        foreach (var card in roomDto.Cards)
        {
            var newCard = new Card() { Id = card.Id, Color = card.Color, Value = card.Value, ImageUri = card.ImageUri };
            room.Cards.Add(newCard);
        }

        return room;
    }
}