using System.Text;
using Newtonsoft.Json;

namespace UNO.Contract;

public class RoomClient
{
    public async Task<string> GetAllRooms()
    {
        var httpClient = new HttpClient();
        var respone = await httpClient.GetAsync("http://localhost:5000/api/Rooms");
        respone.EnsureSuccessStatusCode();

        var gettedRooms = await respone.Content.ReadAsStringAsync();
        return gettedRooms;
    }

    private async Task GetPlayers()
    {
        var httpClient = new HttpClient();
        var respone = await httpClient.GetAsync("http://localhost:5000/api/Player");
        respone.EnsureSuccessStatusCode();

        var gettedLobbies = await respone.Content.ReadAsStringAsync();
        var players = JsonConvert.DeserializeObject<List<PlayerDTO>>(gettedLobbies);
    }

    public async Task AddPlayer(RoomDTO roomToUpdate, string playerName)
    {
        var httpClient = new HttpClient();
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/addPlayer/{playerName}";

        var response = await httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async void StartRoom(RoomDTO room)
    {
        var httpClient = new HttpClient();
        var jsonContent = JsonConvert.SerializeObject(room);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/startroom/{room.Id}";

        var response = await httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task PlaceCard(string card, RoomDTO roomToUpdate)
    {
        var httpClient = new HttpClient();
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/placecard/{card}";

        var response = await httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }
    public async Task PlayerEndMove(int playerId, RoomDTO roomToUpdate)
    {
        var httpClient = new HttpClient();
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/playerendmove/{playerId}";

        var response = await httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task DrawCard(string playername, RoomDTO roomToUpdate)
    {
        var httpClient = new HttpClient();
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/drawCard/{playername}";

        var response = await httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateMaximalPlayers(RoomDTO roomToUpdate, int selectedMaximalCount)
    {
        var httpClient = new HttpClient();
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/updatemaximalplayers/{selectedMaximalCount}";

        var response = await httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemovePlayer(RoomDTO roomToUpdate, int id)
    {
        await GetPlayers();

        var httpClient = new HttpClient();
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/removePlayer/{id}";

        var response2 = await httpClient.PutAsync(addPlayerUrl, httpContent);
        response2.EnsureSuccessStatusCode();
        

        var removePlayerUrl = $"http://localhost:5000/api/Player/{id}";

        var response = await httpClient.DeleteAsync(removePlayerUrl);
        response.EnsureSuccessStatusCode();
    }

    public async Task<RoomDTO> PostRoomAsync(RoomDTO room)
    {
        try
        {
            var httpClient = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(room);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"http://localhost:5000/api/Rooms", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RoomDTO>(responseString);
            }
        }
        catch (Exception e)
        {
            Console.Write(e);
            // ignored
        }

        return room;
    }
}