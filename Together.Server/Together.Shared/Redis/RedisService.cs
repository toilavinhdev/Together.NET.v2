using StackExchange.Redis;
using Together.Shared.Constants;
using Together.Shared.Extensions;

namespace Together.Shared.Redis;

public interface IRedisService
{
    Task<string?> StringGetAsync(TogetherRedisKey key);
    
    Task<T?> StringGetAsync<T>(TogetherRedisKey key);

    Task<bool> StringSetAsync(TogetherRedisKey key, string value, TimeSpan? expiry = null);
    
    Task<bool> StringSetAsync<T>(TogetherRedisKey key, T value, TimeSpan? expiry = null);

    Task ListLeftPushAsync(TogetherRedisKey key, string value);

    Task<RedisValue?> ListLeftPopAsync(TogetherRedisKey key);

    Task ListRightPushAsync(TogetherRedisKey key, string value);

    Task<RedisValue?> ListRightPopAsync(TogetherRedisKey key);

    Task<RedisValue[]> ListRangeAsync(TogetherRedisKey key, long start, long stop);

    Task<bool> SetAddAsync(TogetherRedisKey key, string value);

    Task<bool> SetRemoveAsync(TogetherRedisKey key, string value);

    Task<bool> SetContainsAsync(TogetherRedisKey key, string value);

    Task<long> SetLengthAsync(TogetherRedisKey key);
    
    Task<long> IncrementAsync(TogetherRedisKey key);

    Task<long> DecrementAsync(TogetherRedisKey key);

    Task<bool> ExistsAsync(TogetherRedisKey key);

    Task<bool> RemoveAsync(TogetherRedisKey key);

    Task Transaction(TogetherRedisDatabase database, Func<ITransaction, Task> callback);
}

public class RedisService(IConnectionMultiplexer connection) : IRedisService
{
    private IDatabase Database(TogetherRedisDatabase db = TogetherRedisDatabase.Default) => connection.GetDatabase((int)db);
    
    public async Task<string?> StringGetAsync(TogetherRedisKey key)
    {
        var value = await Database(key.Database).StringGetAsync(key.KeyName);
        return value.ToString();
    }

    public async Task<T?> StringGetAsync<T>(TogetherRedisKey key)
    {
        var value = await StringGetAsync(key);
        return value!.ToObject<T>();
    }

    public async Task<bool> StringSetAsync(TogetherRedisKey key, string value, TimeSpan? expiry = null)
    {
        return await Database(key.Database).StringSetAsync(key.KeyName, value, expiry);
    }

    public async Task<bool> StringSetAsync<T>(TogetherRedisKey key, T value, TimeSpan? expiry = null)
    {
        return await Database(key.Database).StringSetAsync(key.KeyName, value.ToJson(), expiry);
    }

    public async Task ListLeftPushAsync(TogetherRedisKey key, string value)
    {
        await Database(key.Database).ListLeftPushAsync(key.KeyName, value);
    }
    
    public async Task<RedisValue?> ListLeftPopAsync(TogetherRedisKey key)
    {
        return await Database(key.Database).ListLeftPopAsync(key.KeyName);
    }
    
    public async Task ListRightPushAsync(TogetherRedisKey key, string value)
    {
        await Database(key.Database).ListRightPushAsync(key.KeyName, value);
    }
    
    public async Task<RedisValue?> ListRightPopAsync(TogetherRedisKey key)
    {
        return await Database(key.Database).ListRightPopAsync(key.KeyName);
    }
    
    public async Task<RedisValue[]> ListRangeAsync(TogetherRedisKey key, long start, long stop)
    {
        return await Database(key.Database).ListRangeAsync(key.KeyName, start, stop);
    }
    
    public async Task<bool> SetAddAsync(TogetherRedisKey key, string value)
    {
        return await Database(key.Database).SetAddAsync(key.KeyName, value);
    }
    
    public async Task<bool> SetRemoveAsync(TogetherRedisKey key, string value)
    {
        return await Database(key.Database).SetRemoveAsync(key.KeyName, value);
    }
    
    public async Task<bool> SetContainsAsync(TogetherRedisKey key, string value)
    {
        return await Database(key.Database).SetContainsAsync(key.KeyName, value);
    }
    
    public async Task<long> SetLengthAsync(TogetherRedisKey key)
    {
        return await Database(key.Database).SetLengthAsync(key.KeyName);
    }
    
    public async Task<long> IncrementAsync(TogetherRedisKey key)
    {
        return await Database(key.Database).StringIncrementAsync(key.KeyName);
    }

    public async Task<long> DecrementAsync(TogetherRedisKey key)
    {
        return await Database(key.Database).StringDecrementAsync(key.KeyName);
    }

    public async Task<bool> ExistsAsync(TogetherRedisKey key)
    {
        return await Database(key.Database).KeyExistsAsync(key.KeyName);
    }

    public async Task<bool> RemoveAsync(TogetherRedisKey key)
    {
        return await Database(key.Database).KeyDeleteAsync(key.KeyName);
    }

    public async Task Transaction(TogetherRedisDatabase database, Func<ITransaction, Task> callback)
    {
        var transaction = Database(database).CreateTransaction();
        await callback(transaction);
        await transaction.ExecuteAsync();
    }
}