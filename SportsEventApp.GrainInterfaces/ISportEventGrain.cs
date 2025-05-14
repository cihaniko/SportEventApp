using Orleans;

namespace SportsEventApp.GrainInterfaces;

public interface ISportEventGrain : IGrainWithIntegerKey
{
    Task RegisterUser(string userName);
    Task<List<string>> GetRegisteredUsers();
}