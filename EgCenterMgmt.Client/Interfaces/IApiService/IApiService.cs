namespace EgCenterMgmt.Client.Interfaces.IApiService
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<T> PutAsync<T>(string endpoint, object data);
        Task DeleteAsync(string endpoint);
        Task<T> HandleApiErrors<T>(Func<Task<T>> apiCall);
    }
}
