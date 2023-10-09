using TicketReservation.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TicketReservation.Services;

public class MongoDBService {
    private readonly IMongoCollection<User> _users;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
       MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
    IMongoDatabase database = client.GetDatabase("New2"); // Use MongoDB.DatabaseName
    _users = database.GetCollection<User>("Users"); // Use MongoDB.CollectionName
    }

    public async Task CreateAsync(User user) {
        await _users.InsertOneAsync(user);
        Console.WriteLine("User created");
        return;
    }


    public async Task<User> loginAsync(string email  , string password) {
        Console.WriteLine("User login");
        var filter = Builders<User>.Filter.Eq("Email", email) & Builders<User>.Filter.Eq("Password", password);
        return await _users.Find(filter).FirstOrDefaultAsync();

    }




    public async Task<List<User>> GetUsersAsync() {
        Console.WriteLine("User get");
        return await _users.Find(new BsonDocument()).ToListAsync();

    }

    // public async Task AddUserAsync ( string id, string userId) {
    //     FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
    //     UpdateDefinition<User> update = Builders<User>.Update.AddToSet<string>("userId", userId);
    //     await _users.UpdateOneAsync(filter, update);
    //     return;
    // }

    //write a method to update a user details
    public async Task UpdateUserAsync(string id, User user) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        UpdateDefinition<User> update = Builders<User>.Update.Set("Username", user.Username).Set("Password", user.Password).Set("Email", user.Email).Set("Nic", user.Nic).Set("Active", user.Active);
        await _users.UpdateOneAsync(filter, update);
        return;
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        await _users.DeleteOneAsync(filter);
        return;
    }

    //deactivate or reactivate user
    public async Task DeactivateAsync(string id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        UpdateDefinition<User> update = Builders<User>.Update.Set("Active", false);
        await _users.UpdateOneAsync(filter, update);
        return;
    }
    
    public async Task ActivateAsync(string id){
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        UpdateDefinition<User> update = Builders<User>.Update.Set("Active", true);
        await _users.UpdateOneAsync(filter, update);
        return;
    }
    
    
}