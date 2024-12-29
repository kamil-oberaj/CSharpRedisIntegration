using StackExchange.Redis;

namespace CSharpRedisIntegration;

public static class Functions
{
    public static async Task CheckConnectionAsync(this IDatabase db)
    {
        ArgumentNullException.ThrowIfNull(db);

        var result = await db.PingAsync();
        
        Console.WriteLine($"Connected to Redis! Ping time: {result}.");
    }
    
    public static async Task PutPersonAsync(this IDatabase db, Person person)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(person);

        var json = System.Text.Json.JsonSerializer.Serialize(person);

        var kvp = new KeyValuePair<RedisKey, RedisValue>(GetPersonKey(person.Id), json);

        await db.StringSetAsync([kvp]);
    }
    
    public static async Task GetPersonAsync(this IDatabase db, Guid id)
    {
        ArgumentNullException.ThrowIfNull(db);

        var person = await db.StringGetAsync(GetPersonKey(id));

        if (person.IsNullOrEmpty)
        {
            throw new Exception($"Redis value was empty: {person.ToString()}");
        }
        
        Console.WriteLine($"Got person from Redis: {person}");
    }

    public static async Task RemovePersonAsync(this IDatabase db, Guid id)
    {
        var key = GetPersonKey(id);

        var result = await db.KeyDeleteAsync(key);
        
        Console.WriteLine($"{(result ? "Deleted" : "Failed to delete")} person with key: {key}");
    }
    
    
    private static RedisKey GetPersonKey(Guid id) => $"person:{id}";
}