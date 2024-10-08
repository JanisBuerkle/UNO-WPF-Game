namespace UNO.Contract;

public interface IRoomClient
{
    Task<string> GetAllRooms();
    Task AddPlayer(RoomDto roomToUpdate, string playerInformations);
    Task UnoClicked(RoomDto roomToUpdate, int playerId);
    void StartRoom(RoomDto room);
    Task PlaceCard(string card, RoomDto roomToUpdate);
    Task PlayerEndMove(int playerId, RoomDto roomToUpdate);
    Task DrawCard(string playername, RoomDto roomToUpdate);
    Task ResetRoom(string playername, RoomDto roomToUpdate);
    Task UpdateMaximalPlayers(RoomDto roomToUpdate, int selectedMaximalCount);
    Task RemovePlayer(RoomDto roomToUpdate, int id);
    Task<RoomDto> PostRoomAsync(RoomDto room);
}