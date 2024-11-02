// Decompiled with JetBrains decompiler
// Type: Program
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost;
using LocalHost.Interfaces;
using LocalHost.Models;
using LocalHost.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

#nullable enable
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ServiceCollectionServiceExtensions.AddSingleton<IConfigService, ConfigService>(services);
ServiceCollectionServiceExtensions.AddTransient<IGameService, LocalGameService>(services);
ServiceCollectionServiceExtensions.AddTransient<IIterationService, IterationService>(services);
ServiceCollectionServiceExtensions.AddTransient<ICustomerService, CustomerService>(services);
ServiceCollectionServiceExtensions.AddTransient<ISaveGameService, SaveGameService>(services);
WebApplication app = builder.Build();
await DataContext.MigrateAsync();
EndpointRouteBuilderExtensions.MapPost(app, "/game", (Delegate)(async (gameService, httpContext, [FromBody] gameInput) =>
{
    (Guid apiKey2, IResult unauthorized2) = CheckAuth(httpContext);
    if (unauthorized2 != null)
        return unauthorized2;
    GameResponse gameResponse = await gameService.RunGame(gameInput, apiKey2);
    return gameResponse.Message != null ? Results.BadRequest<string>(gameResponse.Message) : Results.Ok<GameResponse>(gameResponse);
}));
EndpointRouteBuilderExtensions.MapGet(app, "/game", (Delegate)(async (gameService, httpContext, [FromQuery] gameId) =>
{
    (Guid apiKey4, IResult unauthorized4) = CheckAuth(httpContext);
    if (unauthorized4 != null)
        return unauthorized4;
    SaveGame game = await gameService.GetGame(gameId, apiKey4);
    return (object)game != null ? Results.Ok<SaveGame>(game) : Results.NotFound();
}));
app.Run();
app = (WebApplication)null;

static (Guid apiKey, IResult? unauthorized) CheckAuth(HttpContext httpContext)
{
    Guid result;
    return Guid.TryParse(Enumerable.FirstOrDefault<string>(httpContext.Request.Headers["x-api-key"]), out result) ? (result, (IResult)null) : (result, Results.Unauthorized());
}
