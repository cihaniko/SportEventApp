using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SportsEventApp.GrainInterfaces;
using SportsEventApp.Infrastructure.DataAccess.Repository;
using SportsEventApp.Infrastructure.Dtos;

namespace SportEventApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportEventsController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ISportEventRepository _repository;

        public SportEventsController(IGrainFactory grainFactory, ISportEventRepository repository)
        {
            _grainFactory = grainFactory;
            _repository = repository;
        }

        /// <summary>
        /// Tüm spor etkinliklerini döner
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _repository.GetAllEventsAsync();
            return Ok(events);
        }

        /// <summary>
        /// Kullanýcýyý etkinliðe kaydeder
        /// </summary>
        [HttpPost("{id}/register")]
        public async Task<IActionResult> RegisterUser(int id, [FromBody] RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                return BadRequest("Kullanýcý adý boþ olamaz.");

            var grain = _grainFactory.GetGrain<ISportEventGrain>(id);
            try
            {
                await grain.RegisterUser(request.UserName);
                return Ok(new { message = "Kayýt baþarýlý." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Belirli bir etkinliðe kayýtlý kullanýcýlarý döner
        /// </summary>
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetRegisteredUsers(int id)
        {
            var grain = _grainFactory.GetGrain<ISportEventGrain>(id);
            var users = await grain.GetRegisteredUsers();
            return Ok(users);
        }
    }

}
