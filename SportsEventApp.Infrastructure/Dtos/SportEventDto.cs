
namespace SportsEventApp.Infrastructure.Dtos;

public class SportEventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime StartTime { get; set; }
    public int Capacity { get; set; }
}
