using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

// static extension methods
public static class GamesEndpoints
{
    const string GetGameByIdEndpointName = "GetGameById";
    private static readonly List<GameDto> games = [
        new GameDto(1, "The Last of Us Part II", "Action-adventure", 59.99m, new DateOnly(2020, 6, 19)),
        new GameDto(2, "Ghost of Tsushima", "Action-adventure", 59.99m, new DateOnly(2020, 7, 17)),
        new GameDto(3, "Cyberpunk 2077", "Action role-playing", 59.99m, new DateOnly(2020, 12, 10)),
    ];

    // extension method
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        // group builder
        var group = app.MapGroup("games");

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) => 
        {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameByIdEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            // not thread-safe
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game);
            
            // provide location header back to client so it can find the newly created resource
            return Results.CreatedAtRoute(GetGameByIdEndpointName, new { id = game.Id }, game);
        });

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => 
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1) {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
        
        return group;
    }
}