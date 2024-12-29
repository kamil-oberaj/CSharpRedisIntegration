using CSharpRedisIntegration;
using StackExchange.Redis;

var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
var db = redis.GetDatabase();

await db.CheckConnectionAsync();

var person = new Person(Guid.NewGuid(), "John Doe", 30);

await db.PutPersonAsync(person);
await db.GetPersonAsync(person.Id);
await db.RemovePersonAsync(person.Id);