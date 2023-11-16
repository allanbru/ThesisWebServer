// See https://aka.ms/new-console-template for more information

using Amazon.SecurityToken.Model;
using System.Text;
using ThesisWebServer;
using Newtonsoft.Json;
using Nancy.Hosting.Self;

public class Program
{

    private static DirectoryInfo? configDir;
    private static string? credentialsPath;
    public static Database? db { get; private set; }

    public static void Start()
    {
        configDir = Directory.CreateDirectory("config");
        credentialsPath = Path.Combine(configDir.FullName, "mongodb.json");
        
        if (!File.Exists(credentialsPath))
        {
            FileStream f = File.Create(credentialsPath);
            string newCfg = JsonConvert.SerializeObject(new MongoDBConfig("user", "pass", "server.com"));
            f.Write(Encoding.ASCII.GetBytes(newCfg));
            f.Close();
        }

        MongoDBConfig cfg = JsonConvert.DeserializeObject<MongoDBConfig>(File.ReadAllText(credentialsPath));

        db = new Database(cfg);
        
        if (db == null)
        {
            throw new Exception("DB can't be null");
        }
    }

    public static void Main(string[] argv)
    {
        Start();
        //WebServer ws = new WebServer();

        Uri uri = new("http://localhost:8080");
        HostConfiguration hostConfigs = new();
        hostConfigs.UrlReservations.CreateAutomatically = true;
        hostConfigs.RewriteLocalhost = true;
        using (var _server = new NancyHost(hostConfigs, uri))
        {
            _server.Start();
            Console.WriteLine("NancyFX Host has started. Press Enter to stop.");
            Console.ReadLine();
        }
    }
}



