using Newtonsoft.Json;
using PythonIntegrated.Data.Domain;

namespace PythonIntegrated.Services
{
    public class HandTrackingService
    {
        private readonly HttpClient _httpClient;

        public HandTrackingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HandPosition> GetHandPositionAsync()
        {
            var response = await _httpClient.GetAsync("http://127.0.0.1:5000/track_hand");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var handPosition = JsonConvert.DeserializeObject<HandPosition>(content);
            return handPosition;
        }
    }
}
