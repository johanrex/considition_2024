using Newtonsoft.Json;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Services
{
    public class ServerUtils
    {
        public string GameUrl;
        public string ApiKey;
        private static readonly HttpClient client = new HttpClient();

        private ServerUtils()
        { }

        public ServerUtils(string gameUrl, string apiKey)
        {
            GameUrl = gameUrl;
            ApiKey = apiKey;
        }

        public async Task<GameResponse> SubmitGameAsync(GameInput input)
        {
            //Must use System.Text.Json since we have objects configured for this serialization. 
            var inputJson = System.Text.Json.JsonSerializer.Serialize(input);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(GameUrl + "game", UriKind.Absolute)))
            {
                request.Headers.Add("x-api-key", ApiKey);
                request.Content = new StringContent(inputJson, Encoding.UTF8, "application/json");

                var res = await client.SendAsync(request);
                var responsePayload = await res.Content.ReadAsStringAsync();
                var gameResponse = JsonConvert.DeserializeObject<GameResponse>(responsePayload);
                return gameResponse;
            }
        }
    }
}