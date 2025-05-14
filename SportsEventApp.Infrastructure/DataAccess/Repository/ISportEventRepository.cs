using SportsEventApp.Infrastructure.Dtos;

namespace SportsEventApp.Infrastructure.DataAccess.Repository;

public interface ISportEventRepository
{
    Task<IEnumerable<SportEventDto>> GetAllEventsAsync();
    Task<SportEventDto> GetByIdAsync(int id);
    Task RegisterUserAsync(int eventId, string userName);
    Task<IEnumerable<string>> GetRegisteredUsersAsync(int eventId);
}
