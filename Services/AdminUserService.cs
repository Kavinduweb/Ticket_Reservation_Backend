using TicketReservation.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TicketReservation.Services;

public class AdminUserService {
    private readonly IMongoCollection<Admin> _admins;

    public AdminUserService(IOptions<MongoDBSettings> mongoDBSettings) {
               MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
    IMongoDatabase database = client.GetDatabase("New3"); // Use MongoDB.DatabaseName
    _admins = database.GetCollection<Admin>("Admin"); // Use MongoDB.CollectionName
    }

    public async Task CreateAsync(Admin admin) {
        await _admins.InsertOneAsync(admin);
        Console.WriteLine("Admin created");
        return;
    }


    public async Task<Admin> loginAsync(string email  , string password) {
        Console.WriteLine("Admin login11");
        var filter = Builders<Admin>.Filter.Eq("Email", email) & Builders<Admin>.Filter.Eq("Password", password);
        return await _admins.Find(filter).FirstOrDefaultAsync();

    }




    public async Task<List<Admin>> GetUsersAsync() {
        Console.WriteLine("Admin get");
        return await _admins.Find(new BsonDocument()).ToListAsync();

    }

    public async Task UpdateUserAsync(string id, Admin admin) {
        FilterDefinition<Admin> filter = Builders<Admin>.Filter.Eq("Id", id);
        UpdateDefinition<Admin> update = Builders<Admin>.Update.Set("Name", admin.Name).Set("Password", admin.Password).Set("Email", admin.Email);
        await _admins.UpdateOneAsync(filter, update);
        return;
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<Admin> filter = Builders<Admin>.Filter.Eq("Id", id);
        await _admins.DeleteOneAsync(filter);
        return;
    }
    
    
}