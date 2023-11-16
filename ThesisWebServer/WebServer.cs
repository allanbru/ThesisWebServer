using Newtonsoft.Json;
using Nancy;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace ThesisWebServer
{
    public class WebServer : NancyModule
    {

        private async Task<dynamic> GetDocuments()
        {
            List<BsonDocument> obj = await Program.db.GetDocuments();
            JsonWriterSettings settings = new JsonWriterSettings{ OutputMode = JsonOutputMode.CanonicalExtendedJson };
            return obj.ToJson(settings);
        }

        public WebServer() : base()
        {
            After.AddItemToEndOfPipeline((ctx) => ctx.Response
            .WithHeader("Access-Control-Allow-Origin", "*")
            .WithHeader("Access-Control-Allow-Methods", "POST,GET")
            .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type"));
            Get("/api", async args => await GetDocuments());
        }
    }
}
