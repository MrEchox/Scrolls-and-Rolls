using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

public static class GameplayEndpoints
{
    public static void MapGameplayEndpoints(this WebApplication app)
    {
        app.MapPost("/sessions/{sessionId}/diceRoll", async (Guid sessionId, DiceRollDto diceRollDto) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(diceRollDto);

            // Validate input DTO
            if (!Validator.TryValidateObject(diceRollDto, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            var diceService = new DiceService();
            var (rolls, total) = diceService.RollDice(diceRollDto.DiceType, diceRollDto.NumberOfDice, diceRollDto.Modifier);

            var diceRollResponse = new
            {
                SessionId = sessionId,
                DiceType = diceRollDto.DiceType,
                Rolls = rolls,
                Total = total,
                Modifier = diceRollDto.Modifier
            };

            // Return the response
            return Results.Ok(diceRollResponse);
        })
        .WithName("RollDice")
        .WithDescription("Rolls dice for specified session.")
        .Produces<object>(StatusCodes.Status200OK) // Adjust the type as needed
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();
    }
}
