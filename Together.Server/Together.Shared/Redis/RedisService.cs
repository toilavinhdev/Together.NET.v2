using StackExchange.Redis;
using Together.Shared.Extensions;

namespace Together.Shared.Redis;

public interface IRedisService
{
    Task<string?> GetAsync(string key);
    
    Task<T?> GetAsync<T>(string key);

    Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null);
    
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    
    Task<long> IncrementAsync(string key);

    Task<long> DecrementAsync(string key);

    Task<bool> ExistsAsync(string key);

    Task<bool> RemoveAsync(string key);

    Task Transaction(Func<ITransaction, Task> callback);
}

public class RedisService(IConnectionMultiplexer connection) : IRedisService
{
    private IDatabase Database(int db = 1) => connection.GetDatabase(db);
    
    public async Task<string?> GetAsync(string key)
    {
        var value = await Database().StringGetAsync(key);
        return value.ToString();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await GetAsync(key);
        return value!.ToObject<T>();
    }

    public async Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        return await Database().StringSetAsync(key, value, expiry);
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        return await Database().StringSetAsync(key, value.ToJson(), expiry);
    }
    
    public async Task<long> IncrementAsync(string key)
    {
        return await Database().StringIncrementAsync(key);
    }

    public async Task<long> DecrementAsync(string key)
    {
        return await Database().StringDecrementAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await Database().KeyExistsAsync(key);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await Database().KeyDeleteAsync(key);
    }

    public async Task Transaction(Func<ITransaction, Task> callback)
    {
        var transaction = Database().CreateTransaction();
        await callback(transaction);
        await transaction.ExecuteAsync();
    }
}