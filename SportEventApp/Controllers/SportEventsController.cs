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
        /// T�m spor etkinliklerini d�ner
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _repository.GetAllEventsAsync();
            return Ok(events);
        }

        /// <summary>
        /// Kullan�c�y� etkinli�e kaydeder
        /// </summary>
        [HttpPost("{id}/register")]
        public async Task<IActionResult> RegisterUser(int id, [FromBody] RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                return BadRequest("Kullan�c� ad� bo� olamaz.");

            var grain = _grainFactory.GetGrain<ISportEventGrain>(id);
            try
            {
                await grain.RegisterUser(request.UserName);
                return Ok(new { message = "Kay�t ba�ar�l�." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Belirli bir etkinli�e kay�tl� kullan�c�lar� d�ner
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
