using DataFeed.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataFeed.Persistence
{
    public interface IUsersRepository
    {
        Task InsertUser(User user);
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetUsersByField(string fieldName, string fieldValue);
        Task<List<User>> GetUsers(int startingFrom, int count);
    }
    //public class UsersRepository : IUsersRepository
    //{
    //    private IMongoClient _client;
    //    private IMongoDatabase _database;
    //    private IMongoCollection<User> _usersCollection;

    //    public UsersRepository(string connectionString)
    //    {
    //        _client = new MongoClient(connectionString);
    //        _database = _client.GetDatabase("blog");
    //        _usersCollection = _database.GetCollection<User>("users");
    //    }

    //    public async Task InsertUser(User user)
    //    {
    //        await _usersCollection.InsertOneAsync(user);
    //    }

    //    public async Task<List<User>> GetAllUsers()
    //    {
    //        return await _usersCollection.Find(new BsonDocument()).ToListAsync();
    //    }

    //    public async Task<List<User>> GetUsersByField(string fieldName, string fieldValue)
    //    {
    //        var filter = Builders<User>.Filter.Eq(fieldName, fieldValue);
    //        var result = await _usersCollection.Find(filter).ToListAsync();

    //        return result;
    //    }

    //    public async Task<List<User>> GetUsers(int startingFrom, int count)
    //    {
    //        var result = await _usersCollection.Find(new BsonDocument())
    //                                           .Skip(startingFrom)
    //                                           .Limit(count)
    //                                           .ToListAsync();

    //        return result;
    //    }
    //}

    public interface IMongoClient
    {
        string Collection { get; set; }
        Task Insert<T>(T item);
        Task InsertMany<T>(IList<T> items);
        Task<IList<T>> GetAll<T>();
        Task<IList<T>> Find<T>(string column, string value);
        Task<IList<T>> Find<T>(string column, List<string> values);
    }
    public class MongoClient : IMongoClient
    {
        readonly MongoDB.Driver.MongoClient client;
        readonly IMongoDatabase db;
        private string col;

        public string Collection { get => col; set => col = value; }

        public MongoClient(MongoOptions o)
        {
            client = new MongoDB.Driver.MongoClient(o.ConnectionString);
            col = o.Collection;
            db = client.GetDatabase(o.Db);
        }

        public async Task<IList<T>> GetAll<T>()
        {
            var c = db.GetCollection<T>(col);
            return (await c.FindAsync(new BsonDocument())).ToList();
        }

        public async Task<IList<T>> Find<T>(string column, string value)
        {
            var c = db.GetCollection<T>(col);
            var filter = Builders<T>.Filter.Eq(column, value);
            return (await c.FindAsync<T>(filter)).ToList();
        }

        public async Task<IList<T>> Find<T>(string column, List<string> values)
        {
            var c = db.GetCollection<T>(col);
            var filter = Builders<T>.Filter.In(column, values);
            return (await c.FindAsync<T>(filter)).ToList();
        }

        public async Task Insert<T>(T item)
        {
            var c = db.GetCollection<T>(col);
            await c.InsertOneAsync(item);
        }

        public async Task InsertMany<T>(IList<T> items)
        {
            var c = db.GetCollection<T>(col);
            await c.InsertManyAsync(items);
        }
    }
}
