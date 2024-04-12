using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;
using UNO.Contract;
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


        

        public void Dispose()
        {
            _httpClient.Dispose();
        }
        
    }
}