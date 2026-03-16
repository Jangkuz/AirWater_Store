using System.ComponentModel.DataAnnotations;

namespace AirWaterStore.Web.Models.Catalog;

public partial class Game
{
    public int Id { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public List<string> Genres { get; set; } = [];

    [Display(Name = "Genres (comma-separated)")]
    public string GenresString
    {
        get
        {
            var result = "N/A.";
            if (Genres != null && Genres.Any())
            {
                result = string.Join(", ", Genres) + ".";
            }
            return result;
        }
        set
        {
            Genres = value?
            .TrimEnd('.')
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList() ?? new List<string>();
        }
    }
    public string? Developer { get; set; }

    public string? Publisher { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}

//wrapper classes
public record GetGamesResponse(IEnumerable<Game> Games);
public record GetGameByIdResponse(Game Game);

public record CreateGameDto(
    string ThumbnailUrl,
    string Title,
    string Description,
    List<string> Genre,
    string Developer,
    string Publisher,
    DateOnly ReleaseDate,
    decimal Price,
    int Quantity
    );

public record CreateGameResponse(int Id);

public record UpdateGameDto(
    int Id,
    string ThumbnailUrl,
    string Title,
    string Description,
    List<string> Genre,
    string Developer,
    string Publisher,
    DateOnly ReleaseDate,
    decimal Price,
    int Quantity
    );

public record UpdateGameResponse(bool IsSuccess);

public record DeleteGameResponse(bool IsSuccess);

public record GetGamesCountResponse(int Count);
