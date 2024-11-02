// Decompiled with JetBrains decompiler
// Type: LocalHost.DataContext
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
// Assembly location: C:\temp\app\LocalHost.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace LocalHost
{
    public class DataContext
    {
        public static async Task MigrateAsync()
        {
            bool result;
            if (bool.TryParse(Environment.GetEnvironmentVariable("DB_DISABLED"), out result) & result)
                return;
            try
            {
                string str = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new Exception("Could not read connection string");
                string input = Environment.GetEnvironmentVariable("CONSIDITION_API_KEY") ?? throw new Exception("Could not read team id");
                Console.WriteLine("DB connection string: " + str);
                Console.WriteLine("API key: " + input);
                Guid apiKey;
                if (!Guid.TryParse(input, out apiKey))
                    throw new Exception("Could not parse api key");
                string[] source = str.Split(";");
                string database = ((IEnumerable<string>)source).First<string>((Func<string, bool>)(x => x.Split("=")[0].Equals("Database", StringComparison.OrdinalIgnoreCase))).Split("=")[1];
                SqlConnection conn = new SqlConnection(string.Join(";", ((IEnumerable<string>)source).Where<string>((Func<string, bool>)(x => !x.Split("=")[0].Equals("database", StringComparison.OrdinalIgnoreCase))).ToArray<string>()));
                conn.Open();
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(197, 2);
                interpolatedStringHandler.AppendLiteral(" IF NOT EXISTS (\r\n        SELECT name \r\n        FROM sys.databases \r\n        WHERE name = '");
                interpolatedStringHandler.AppendFormatted(database);
                interpolatedStringHandler.AppendLiteral("'\r\n    )\r\n    BEGIN\r\n        CREATE DATABASE ");
                interpolatedStringHandler.AppendFormatted(database);
                interpolatedStringHandler.AppendLiteral("\r\n        COLLATE Latin1_General_100_CI_AS_SC_UTF8;\r\n    END;");
                int num1 = await conn.ExecuteAsync(interpolatedStringHandler.ToStringAndClear(), commandTimeout: new int?(10));
                await conn.ChangeDatabaseAsync(database);
                int num2 = await conn.ExecuteAsync("    IF NOT EXISTS (\r\n        SELECT * \r\n        FROM INFORMATION_SCHEMA.TABLES \r\n        WHERE TABLE_NAME = 'Teams'\r\n    )\r\n    BEGIN\r\n        CREATE TABLE Teams (\r\n            Id UNIQUEIDENTIFIER PRIMARY KEY,\r\n            Name NVARCHAR(255) NOT NULL,\r\n            Venue NVARCHAR(255) NULL,\r\n            ApiKey UNIQUEIDENTIFIER NOT NULL UNIQUE\r\n        );\r\n   END;\r\n    \r\n   IF NOT EXISTS (\r\n       SELECT * \r\n       FROM INFORMATION_SCHEMA.TABLES \r\n       WHERE TABLE_NAME = 'SavedGame'\r\n   )\r\n   BEGIN\r\n       CREATE TABLE SavedGame (\r\n           Id UNIQUEIDENTIFIER PRIMARY KEY,\r\n           TeamId UNIQUEIDENTIFIER NOT NULL,\r\n           GameData NVARCHAR(MAX) NOT NULL,\r\n           CONSTRAINT FK_SavedGame_Team FOREIGN KEY (TeamId) REFERENCES Teams(Id)\r\n       );\r\n   END;\r\n    \r\n     IF NOT EXISTS (\r\n     SELECT * \r\n     FROM INFORMATION_SCHEMA.TABLES \r\n     WHERE TABLE_NAME = 'Leaderboard'\r\n     )\r\n     BEGIN\r\n         CREATE TABLE Leaderboard (\r\n             Id UNIQUEIDENTIFIER PRIMARY KEY,\r\n             GameId UNIQUEIDENTIFIER NOT NULL,\r\n             TeamId UNIQUEIDENTIFIER NOT NULL,\r\n             City NVARCHAR(255) NOT NULL,\r\n             TotalScore BIGINT NOT NULL,\r\n             Happiness BIGINT NOT NULL,\r\n             EnvironmentalScore BIGINT NOT NULL,\r\n             CONSTRAINT FK_Leaderboard_Team FOREIGN KEY (TeamId) REFERENCES Teams(Id),\r\n             CONSTRAINT FK_Leaderboard_SavedGame FOREIGN KEY (GameId) REFERENCES SavedGame(Id)\r\n         );\r\n    END;", commandTimeout: new int?(10));
                if (await conn.ExecuteScalarAsync<int>("select count(*) from Teams where ApiKey = @ApiKey", (object)new
                {
                    ApiKey = apiKey
                }) > 0)
                    return;
                int num3 = await conn.ExecuteAsync("insert into Teams(id, name, venue, apikey) values (newid(), 'DevelopmentTeam', 'Local', @ApiKey);", (object)new
                {
                    ApiKey = apiKey
                });
                apiKey = new Guid();
                database = (string)null;
                conn = (SqlConnection)null;
            }
            catch (Exception ex)
            {
                throw new Exception("Migration failed", ex);
            }
        }
    }
}
