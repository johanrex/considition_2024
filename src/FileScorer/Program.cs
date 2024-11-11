using Common.Models;
using Common.Services;
using Newtonsoft.Json;
using System.Text;

//string gameUrl = "https://api.considition.com/";
string gameUrl = "http://localhost:8080/";
string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
string mapName = "Almhult";


async Task<GameResponse> SubmitGameAsync(GameInput input)
{
    //var inputJson = JsonConvert.SerializeObject(input);

    //Must use System.Text.Json since we have objects configured to to the serialization. 
    var inputJson = System.Text.Json.JsonSerializer.Serialize(input);

    //if (json != inputJson)
    //    throw new Exception();

    HttpClient client = new HttpClient();

    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(gameUrl + "game", UriKind.Absolute)))
    {
        request.Headers.Add("x-api-key", apiKey);
        request.Content = new StringContent(inputJson, Encoding.UTF8, "application/json");

        var res = await client.SendAsync(request);
        var responsePayload = await res.Content.ReadAsStringAsync();
        var gameResponse = JsonConvert.DeserializeObject<GameResponse>(responsePayload);
        return gameResponse;
    }
}

var text = File.ReadAllText("negativescore.json");
var input = System.Text.Json.JsonSerializer.Deserialize<GameInput>(text);
//var input = JsonConvert.DeserializeObject<GameInput>(text);


var gameResponseRemote = SubmitGameAsync(input).Result;
var totalScoreRemote = gameResponseRemote.Score.TotalScore;
Console.WriteLine(totalScoreRemote);

ConfigService configService = new();
var scorer = new NativeScorer.NativeScorer(configService);
var gameResponseNative = scorer.RunGame(input);
var totalScoreNative = gameResponseNative.Score.TotalScore;
Console.WriteLine(totalScoreNative);


int i = 0;

//if (Math.Abs(serverTotalScore - totalScore) > 5)
//{
//    throw new Exception();
//}

