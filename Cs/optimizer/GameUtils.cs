using Newtonsoft.Json;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class GameUtils
    {
        public string GameUrl;
        public string ApiKey;

        private GameUtils()
        {}

        public GameUtils(string gameUrl, string apiKey)
        {
            GameUrl = gameUrl;
            ApiKey = apiKey;
        }

        static void PrettyPrintJson(object obj)
        {
            string prettyJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Console.WriteLine(prettyJson);
        }

        public static MapData GetMap(string mapFilename)
        {
            string mapDataText = File.ReadAllText(mapFilename);
            var map = JsonConvert.DeserializeObject<MapData>(mapDataText);
            return map;
        }


        public double ScoreGame(GameInput game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            GameResponse resp = SubmitGame(game).Result;

            //TODO this might not always have a score if something bad happened.
            GameResult result = resp.Score;
            return result.TotalScore;
        }

        async public Task<GameResponse> SubmitGame(GameInput input)
        {
            //Console.WriteLine("Request payload:");
            //PrettyPrintJson(input);

            ////TODO this kills performance.
            ////write the json to file
            //string prettyJson = JsonConvert.SerializeObject(input, Formatting.Indented);
            //File.WriteAllText("gameInput.json", prettyJson);

            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(GameUrl + "game", UriKind.Absolute)
            };
            request.Headers.Add("x-api-key", ApiKey);
            request.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

            HttpClient client = new();

            var res = client.Send(request);
            //Console.WriteLine("Response:");
            //Console.WriteLine("");
            //Console.WriteLine(res.StatusCode);

            var responsePayload = await res.Content.ReadAsStringAsync();

            //PrettyPrintJson(JsonConvert.DeserializeObject(responsePayload));

            GameResponse gameResponse = JsonConvert.DeserializeObject<GameResponse>(responsePayload);

            return gameResponse;
        }
    }
}
