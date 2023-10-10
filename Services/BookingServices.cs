using TicketReservation.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketReservation.Services
{
    public class BookingService
    {
        private readonly IMongoCollection<BookingDetails> _bookingDetails;

        public BookingService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase("TrainGoDB"); // Use MongoDB.DatabaseName
            _bookingDetails = database.GetCollection<BookingDetails>("Bookings"); // Use MongoDB.CollectionName
        }

        public async Task CreateAsync(BookingDetails bookingDetails)
        {
            await _bookingDetails.InsertOneAsync(bookingDetails);
            Console.WriteLine("Booking Added Successfully");
        }

        public async Task<List<BookingDetails>> GetBookingsAsync()
        {
            Console.WriteLine("Getting all Bookings");
            return await _bookingDetails.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<BookingDetails> GetBookingDetailsByIdAsync(string id)
        {
            Console.WriteLine($"Getting Booking by ID: {id}");
            var filter = Builders<BookingDetails>.Filter.Eq("Id", id);
            return await _bookingDetails.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateBookingAsync(string id, BookingDetails bookingDetails)
        {
            Console.WriteLine($"Updating train with ID: {id}");
            FilterDefinition<BookingDetails> filter = Builders<BookingDetails>.Filter.Eq("Id", id);
            UpdateDefinition<BookingDetails> update = Builders<BookingDetails>.Update
                .Set("CusName", bookingDetails.CusName)
                .Set("Bookdate", bookingDetails.Bookdate)
                .Set("From", bookingDetails.From)
                .Set("To", bookingDetails.To)
                .Set("Traintime", bookingDetails.Traintime);
            await _bookingDetails.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string id)
        {
            Console.WriteLine($"Deleting Booking with ID: {id}");
            FilterDefinition<BookingDetails> filter = Builders<BookingDetails>.Filter.Eq("Id", id);
            await _bookingDetails.DeleteOneAsync(filter);
        }
    }
}
