using Contract;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Core.Caching
{
    public class KeyStore : IKeyStore
    {
        private readonly IDatabase _database;
        public KeyStore(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public bool AddKey(string key, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddKeyWithExpiryAsync(string key, string value, long miliseconds)
        {
            var created = await _database.StringSetAsync(key,value, TimeSpan.FromMilliseconds(miliseconds));

            return created;
        }
        
        public async Task<bool> KeyExistsAsync(string key)
        {
            var exits = await _database.KeyExistsAsync(key);

            return exits;
        }
        
        public async Task<bool> AddKeyAsync(string key, string value)
        {
            var created = await _database.StringSetAsync(key,value);
            
            return created;
        }

        public string GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetValueAsync(string key)
        {
            var data = await _database.StringGetAsync(key);

            return !data.IsNullOrEmpty ? (string?) data : null;
        }

        public bool RemoveKey(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveKeyAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }
    }
}
