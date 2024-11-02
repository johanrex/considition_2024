// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.SaveGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
// Assembly location: C:\temp\app\LocalHost.dll

using Dapper;
using LocalHost.Interfaces;
using LocalHost.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

#nullable enable
namespace LocalHost.Services
{
    public class SaveGameService : ISaveGameService
    {
        private readonly IConfiguration mConfiguration;
        private const string GetTeamIdSql = "select Id from Teams where ApiKey = @ApiKey";
        private const string GetTopScoreSql = "select Id, max(TotalScore) from Leaderboard where TeamId = @TeamId group by Id";
        private const string InsertTopScoreSql = "insert into Leaderboard(Id, GameId, TeamId, City, TotalScore, Happiness, EnvironmentalScore) values (newid(), @GameId, @TeamId, @City, @TotalScore, @Happiness, @EnviromentalScore);";
        private const string SaveGameSql = "insert into SavedGame(Id, TeamId, GameData) values (@Id, @TeamId, @GameData);";
        private const string GetGameSql = "select GameData from SavedGame where Id = @Id and TeamId = @TeamId;";
        private readonly bool dbDisabled;

        public SaveGameService(IConfiguration configuration)
        {
            this.mConfiguration = configuration;
            bool result;
            if (!bool.TryParse(Environment.GetEnvironmentVariable("DB_DISABLED"), out result))
                return;
            this.dbDisabled = result;
        }

        public async Task<GameResult> SaveGame(
          GameInput gameInput,
          List<Customer> customers,
          Guid apiKey,
          Guid gameId)
        {
            GameResult gameResult = new GameResult()
            {
                TotalProfit = customers.Sum<Customer>((Func<Customer, double>)(x => x.Profit)),
                HappynessScore = customers.Sum<Customer>((Func<Customer, double>)(x => x.Happiness)),
                EnvironmentalImpact = customers.Sum<Customer>((Func<Customer, double>)(x => x.Loan.EnvironmentalImpact)),
                MapName = gameInput.MapName
            };
            if (this.dbDisabled)
                return gameResult;
            Guid teamId;
            await using (SqlConnection connection = new SqlConnection(this.GetDbConnectionString()))
            {
                teamId = await SaveGameService.GetTeamId(connection, apiKey);
                await SaveGameService.SaveGame(connection, teamId, new LocalHost.Models.SaveGame(gameInput, customers, gameResult), gameId);
                await this.SaveTopScore(connection, teamId, gameResult, gameId);
                return gameResult;
            }
            gameResult = (GameResult)null;
            connection = (SqlConnection)null;
            teamId = new Guid();
            GameResult gameResult1;
            return gameResult1;
        }

        public async Task<LocalHost.Models.SaveGame?> GetGame(Guid gameId, Guid apiKey)
        {
            await using (SqlConnection connection = new SqlConnection(this.GetDbConnectionString()))
            {
                string json = await connection.QueryFirstOrDefaultAsync<string>("select GameData from SavedGame where Id = @Id and TeamId = @TeamId;", (object)new
                {
                    Id = gameId,
                    TeamId = await SaveGameService.GetTeamId(connection, apiKey)
                });
                return json == null ? (LocalHost.Models.SaveGame)null : JsonSerializer.Deserialize<LocalHost.Models.SaveGame>(json);
            }
        }

        private static async Task<Guid> GetTeamId(SqlConnection connection, Guid apiKey)
        {
            return await connection.QueryFirstOrDefaultAsync<Guid>("select Id from Teams where ApiKey = @ApiKey", (object)new
            {
                ApiKey = apiKey.ToString()
            });
        }

        private async Task SaveTopScore(
          SqlConnection connection,
          Guid teamId,
          GameResult gameResult,
          Guid gameId)
        {
            (Guid, long)? nullable = await connection.QueryFirstOrDefaultAsync<(Guid, long)?>("select Id, max(TotalScore) from Leaderboard where TeamId = @TeamId group by Id", (object)new
            {
                TeamId = teamId
            });
            if (nullable.HasValue && (double)nullable.Value.Item2 >= gameResult.TotalScore)
                return;
            int num = await connection.ExecuteAsync("insert into Leaderboard(Id, GameId, TeamId, City, TotalScore, Happiness, EnvironmentalScore) values (newid(), @GameId, @TeamId, @City, @TotalScore, @Happiness, @EnviromentalScore);", (object)new
            {
                Id = Guid.NewGuid(),
                GameId = gameId,
                TeamId = teamId,
                City = gameResult.MapName,
                TotalScore = (long)gameResult.TotalScore,
                Happiness = (long)gameResult.HappynessScore,
                EnviromentalScore = (long)gameResult.EnvironmentalImpact
            });
        }

        private static async Task SaveGame(
          SqlConnection connection,
          Guid teamId,
          LocalHost.Models.SaveGame gameResult,
          Guid gameId)
        {
            int num = await connection.ExecuteAsync("insert into SavedGame(Id, TeamId, GameData) values (@Id, @TeamId, @GameData);", (object)new
            {
                Id = gameId,
                TeamId = teamId,
                GameData = JsonSerializer.Serialize<LocalHost.Models.SaveGame>(gameResult)
            });
        }

        private string GetDbConnectionString()
        {
            string connectionString = this.mConfiguration.GetConnectionString("Default");
            if (connectionString != null)
                return connectionString;
            return Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new Exception("Could not read connection string");
        }
    }
}
