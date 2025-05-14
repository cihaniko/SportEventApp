
using Dapper;
using Orleans;
using Orleans.Runtime;
using SportsEventApp.GrainInterfaces;
using SportsEventApp.Grains.States;
using SportsEventApp.Infrastructure.DataAccess.Repository;
using SportsEventApp.Infrastructure.DataAccess.Repository.Base;
using SportsEventApp.Infrastructure.Dtos;
using System.Data;
using static SportsEventApp.Grains.Grains.SportEventGrain;

namespace SportsEventApp.Grains.Grains;

public class SportEventGrain : Grain, ISportEventGrain
{
    private readonly ISportEventRepository _repository;
    private readonly IPersistentState<SportEventState> _state;

    public SportEventGrain(
        ISportEventRepository repository,
        [PersistentState("sportEvent", "SportEventStore")] IPersistentState<SportEventState> state)
    {
        _repository = repository;
        _state = state;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        var eventId = (int)this.GetPrimaryKeyLong();

        if (_state.State.EventInfo == null)
        {
            var eventInfo = await _repository.GetByIdAsync(eventId);

            if (eventInfo == null)
                throw new InvalidOperationException($"SportEvent ID {eventId} veritabanýnda bulunamadý.");

            _state.State.EventInfo = eventInfo;
            await _state.WriteStateAsync();
        }

        await base.OnActivateAsync(cancellationToken);
    }

    public async Task RegisterUser(string userName)
    {
        var eventId = (int)this.GetPrimaryKeyLong();
        var info = _state.State.EventInfo;
        var users = _state.State.RegisteredUsers;

        if (info == null)
            throw new InvalidOperationException("Etkinlik verisi yüklenemedi.");

        if (users.Contains(userName, StringComparer.OrdinalIgnoreCase))
            return;

        if (users.Count >= info.Capacity)
            throw new InvalidOperationException("Etkinlik dolu!");

        // MSSQL'e yaz
        await _repository.RegisterUserAsync(eventId, userName);

        // Memory state'e yaz
        users.Add(userName);
        _state.State.LastUpdated = DateTime.UtcNow;
        _state.State.IsUsersLoaded = true;
        await _state.WriteStateAsync();
    }

    public async Task<List<string>> GetRegisteredUsers()
    {
        if (!_state.State.IsUsersLoaded)
        {
            var eventId = (int)this.GetPrimaryKeyLong();
            var registered = await _repository.GetRegisteredUsersAsync(eventId);
            _state.State.RegisteredUsers = registered.ToList();
            _state.State.IsUsersLoaded = true;
            await _state.WriteStateAsync();
        }

        return _state.State.RegisteredUsers;
    }
    public class PingGrain : Grain, IPingGrain
    {
        public Task Ping() => Task.CompletedTask;
    }
}
