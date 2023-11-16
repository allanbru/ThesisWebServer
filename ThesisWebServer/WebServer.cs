using Newtonsoft.Json;
using Nancy;
using MongoDB.Bson;

namespace ThesisWebServer
{
    public class WebServer : NancyModule
    {

        private async Task<dynamic> GetDocuments()
        {
            List<BsonDocument> obj = await Program.db.GetDocuments();
            return obj.ToJson();
        }

        public WebServer() : base()
        {
            Get("/", async args => await GetDocuments());
        }
    }
}
