using EgCenterMgmt.Client.Interfaces.IApiService;
using System.Net.Http.Json;

namespace EgCenterMgmt.Client.Services.ApiService
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await HandleApiErrors(async () => await _httpClient.GetFromJsonAsync<T>(endpoint));
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            return await HandleApiErrors(async () => await response.Content.ReadFromJsonAsync<T>());
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            return await HandleApiErrors(async () => await response.Content.ReadFromJsonAsync<T>());
        }

        public async Task DeleteAsync(string endpoint)
        {
            await HandleApiErrors(async () =>
            {
                await _httpClient.DeleteAsync(endpoint);
                return Task.CompletedTask;
            });
        }

        public async Task<T> HandleApiErrors<T>(Func<Task<T>> apiCall)
        {
            try
            {
                return await apiCall();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                throw;
            }
        }
    }
}
