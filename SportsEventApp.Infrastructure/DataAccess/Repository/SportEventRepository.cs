using Dapper;
using SportsEventApp.Infrastructure.DataAccess.Repository;
using SportsEventApp.Infrastructure.DataAccess.Repository.Base;
using SportsEventApp.Infrastructure.Dtos;
using System.Data;

public class SportEventRepository : ISportEventRepository
{
    private readonly IDbConnectionFactory _factory;

    public SportEventRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<SportEventDto>> GetAllEventsAsync()
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<SportEventDto>(
            "sp_GetAllSportEvents", 
            commandType: CommandType.StoredProcedure);
    }
    public async Task<SportEventDto> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        var response = await connection.QueryFirstOrDefaultAsync<SportEventDto>(
            "sp_GetSportEventById",
            parameters,
            commandType: CommandType.StoredProcedure);

        return response;
    }

    public async Task RegisterUserAsync(int eventId, string userName)
    {
        using var connection = _factory.CreateConnection();

        await connection.ExecuteAsync(
            "sp_RegisterUserToEvent",
            new { EventId = eventId, UserName = userName },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<string>> GetRegisteredUsersAsync(int eventId)
    {
        using var connection = _factory.CreateConnection();

        var result = await connection.QueryAsync<string>(
            "sp_GetRegisteredUsersByEventId",
            new { EventId = eventId },
            commandType: CommandType.StoredProcedure);

        return result;
    }
}