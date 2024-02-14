using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Service
{
    public class ApiService
    {
        public MainViewModel MainViewModel { get; set; }
        private readonly HttpClient _httpClient;
        
        public ApiService()
        {
            _httpClient = new HttpClient();
        }


        public async Task PostRoomAsync(Rooms room)
        {
            try
            {
                var json = JsonSerializer.Serialize(room);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"http://localhost:5221/api/Rooms", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    // Process the successful response if needed.
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
        
        public async Task PostPlayerAsync(Player player, string list)
        {
            try
            {
                var json = JsonSerializer.Serialize(player);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"http://localhost:5221/api/{list}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    // Process the successful response if needed.
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