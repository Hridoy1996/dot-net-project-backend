namespace Contract
{
    public interface IKeyStore
    {
        Task<bool> AddKeyAsync(string key, string value);
        Task<bool> AddKeyWithExpiryAsync(string key, string value, long miliseconds);
        Task<bool> RemoveKeyAsync(string key);
        bool RemoveKey(string key);
        string GetValue(string key);
        Task<string> GetValueAsync(string key);
    }
}
