
using LocalHost;
using LocalHost.Interfaces;
using LocalHost.Services;
using System.Diagnostics.Metrics;


string content = File.ReadAllText("files/negativescore.json");
var gameInput = System.Text.Json.JsonSerializer.Deserialize<LocalHost.Models.GameInput>(content);

IConfigService configService = new ConfigService();
ICustomerService customerService = new CustomerService(configService);
IterationService iterationService = new IterationService(configService);
ISaveGameService saveGameService = new SaveGameService(null);

//Server = localhost; Database = master; Trusted_Connection = True;

//Environment.SetEnvironmentVariable("DB_DISABLED", "true");
//Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "Server=db;Database=Considition2024;User Id=sa;Password=Kalle123!;TrustServerCertificate=True;");
Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "Server=jbattlestation;Database=Considition2024;User Id=user1;Password=user1;TrustServerCertificate=True;");


//05ae5782-1936-4c6a-870b-f3d64089dcf5
Environment.SetEnvironmentVariable("CONSIDITION_API_KEY", "05AE5782-1936-4C6A-870B-F3D64089DCF5");


Task task = DataContext.MigrateAsync();
task.Wait();


bool result;
Console.WriteLine(bool.TryParse("true", out result));

LocalGameService localGameService = new LocalGameService(
    customerService,
    iterationService,
    saveGameService,
    configService);

var gameResult = localGameService.RunGame(gameInput, Guid.Empty).Result;
Console.WriteLine(gameResult.Score); 


Console.WriteLine("Hello, World!");
