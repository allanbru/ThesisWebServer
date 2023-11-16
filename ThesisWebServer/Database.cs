using MongoDB.Bson;
using MongoDB.Driver;
using Nancy.Json;
using Nancy.ViewEngines.SuperSimpleViewEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ThesisWebServer
{
    public struct WebsiteResult
    {
        public string domain;
        public string mainDomain;
        public string screenshotPath;
    }
    
    public struct MongoDBConfig
    {
        public string username { get; set; }
        public string password { get; set; }
        public string url { get; set; }

        public MongoDBConfig(string username, string password, string url)
        {
            this.username = username;
            this.password = password;
            this.url = url;
        }
    }

    public class Database
    {

        internal class ConnectionFailed : Exception {
            public ConnectionFailed(string message) : base (message) {}
        }

        private MongoDBConfig config;
        private IMongoClient client;
        private IMongoDatabase db;
        private IMongoCollection<BsonDocument> col;

        public Database(MongoDBConfig cfg)
        {
            string conn = $"mongodb+srv://{cfg.username}:{cfg.password}@{cfg.url}";
            try
            {
                client = new MongoClient(conn);
                db = client.GetDatabase("Thesis");
                col = db.GetCollection<BsonDocument>("Crawler");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to connect to cluster: {ex.Message}");
                throw new ConnectionFailed("Could not connect to cluster");
            }
        }

        public async Task<List<BsonDocument>> GetDocuments()
        {
            var filter = Builders<BsonDocument>.Filter.Ne<string?>("screenshot_file_path", null);
            IAsyncCursor<BsonDocument> k = await col.FindAsync<BsonDocument>(filter);
            return k.ToList();
        }
    }
}
