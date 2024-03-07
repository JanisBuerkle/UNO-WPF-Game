using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UNO_Spielprojekt.Service
{
    public class ApiService
    {
        public MainViewModel MainViewModel { get; set; }
        private readonly HttpClient _httpClient;
        private readonly MainViewModel _mainViewModel;

        public ApiService(MainViewModel mainViewModel)
        {
            _httpClient = new HttpClient();
            _mainViewModel = mainViewModel;
        }


        public async Task PostRoomAsync(Rooms room)
        {
            try
            {
                var json = JsonSerializer.Serialize(room);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"http://localhost:5000/api/Rooms", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    _mainViewModel.MultiplayerRoomsViewModel.SelectedRoom2 =
                        JsonConvert.DeserializeObject<Rooms>(responseString);
                }
                else
                {
                    // Handle non-successful response (e.g., log the error).
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the exception).
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
        
    }
}