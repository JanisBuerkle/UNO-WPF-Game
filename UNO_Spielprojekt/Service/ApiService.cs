using System.Net.Http;
using UNO_Spielprojekt.Window;

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