// See https://aka.ms/new-console-template for more information

using Amazon.SecurityToken.Model;
using System.Text;
using ThesisWebServer;
using Newtonsoft.Json;
using Nancy.Hosting.Self;
using Nancy;

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

        string localhost = (argv.Length > 0) ? argv[0] : "127.0.0.1";
        string port = (argv.Length > 1) ? argv[1] : "8080";

        Uri uri = new($"http://{localhost}:{port}");
        HostConfiguration hostConfigs = new();
        hostConfigs.UrlReservations.CreateAutomatically = true;
        hostConfigs.RewriteLocalhost = true;

        using var _server = new NancyHost(hostConfigs, uri);
        
        if (_server == null)
        {
            Console.WriteLine("Server is null.");
            return;
        }

        _server.Start();
        Console.WriteLine("NancyFX Host has started. Press Ctrl+C to stop.");
        while(true)
        {
            Console.ReadLine();
        }
    }
}



