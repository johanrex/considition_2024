// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.SaveGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDC2938F-C917-4854-87EA-D677106BD5FA
// Assembly location: C:\temp\app\LocalHost.dll

using Dapper;
using LocalHost.Interfaces;
using LocalHost.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Threading.Tasks;

#nullable enable
namespace LocalHost.Services
{
    public class SaveGameService : ISaveGameService
    {
        private readonly IConfiguration mConfiguration;
        private const string GetTeamIdSql = "select Id from Teams where ApiKey = @ApiKey";
        private const string GetTopScoreSql = "select Id, max(TotalScore) from Leaderboard where TeamId = @TeamId AND City = @City group by Id";
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

        public async Task SaveGame(
          GameInput gameInput,
          GameResult gameResult,
          List<Customer> customers,
          Guid apiKey,
          Guid gameId)
        {
            SqlConnection connection;
            if (this.dbDisabled)
            {
                connection = (SqlConnection)null;
            }
            else
            {
                connection = new SqlConnection(this.GetDbConnectionString());
                object obj = (object)null;
                int num = 0;
                Guid teamId;
                try
                {
                    teamId = await SaveGameService.GetTeamId(connection, apiKey);
                    await SaveGameService.SaveGame(connection, teamId, new LocalHost.Models.SaveGame(gameInput, customers, gameResult), gameId);
                    await this.SaveTopScore(connection, teamId, gameResult, gameId);
                    num = 1;
                }
                catch (object ex)
                {
                    obj = ex;
                }
                if (connection != null)
                    await connection.DisposeAsync();
                object obj1 = obj;
                if (obj1 != null)
                {
                    if (!(obj1 is Exception source))
                        throw obj1;
                    ExceptionDispatchInfo.Capture(source).Throw();
                }
                if (num == 1)
                {
                    connection = (SqlConnection)null;
                }
                else
                {
                    obj = (object)null;
                    connection = (SqlConnection)null;
                    teamId = new Guid();
                    connection = (SqlConnection)null;
                }
            }
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
            (Guid, long)? nullable = await connection.QueryFirstOrDefaultAsync<(Guid, long)?>("select Id, max(TotalScore) from Leaderboard where TeamId = @TeamId AND City = @City group by Id", (object)new
            {
                TeamId = teamId,
                City = gameResult.MapName
            });
            if (nullable.HasValue && nullable.Value.Item2 >= gameResult.TotalScore)
                return;
            int num = await connection.ExecuteAsync("insert into Leaderboard(Id, GameId, TeamId, City, TotalScore, Happiness, EnvironmentalScore) values (newid(), @GameId, @TeamId, @City, @TotalScore, @Happiness, @EnviromentalScore);", (object)new
            {
                Id = Guid.NewGuid(),
                GameId = gameId,
                TeamId = teamId,
                City = gameResult.MapName,
                TotalScore = gameResult.TotalScore,
                Happiness = gameResult.HappinessScore,
                EnviromentalScore = gameResult.EnvironmentalImpact
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
