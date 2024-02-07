using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UNO_Spielprojekt.AddPlayer;

namespace UNO_Spielprojekt.Service;

public class ApiService
{
    public async Task PostPlayerAsync(Player player)
    {
        var client = new HttpClient();

        var json = JsonSerializer.Serialize(player);

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://localhost:5221/api/Players", content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
        }
        else
        {
        }
    }
}