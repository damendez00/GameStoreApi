using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// app is of type WebApplication
// WebApplication is the host of the application
// Host introduces an HTTP server implementation to the application

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
