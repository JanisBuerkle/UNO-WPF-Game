using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UNO_Spielprojekt.AddPlayer;

namespace UNO_Spielprojekt.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            // You can configure additional settings for HttpClient here if needed.
        }

        public async Task<List<Player>> GetPlayersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5221/api/Players");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var players = JsonSerializer.Deserialize<List<Player>>(json);

                    return players;
                }
                else
                {
                    // Handle non-successful response (e.g., log the error).
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the exception).
                return null;
            }
        }

        public async Task PostPlayerAsync(Player player)
        {
            try
            {
                var json = JsonSerializer.Serialize(player);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:5221/api/Players", content);

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