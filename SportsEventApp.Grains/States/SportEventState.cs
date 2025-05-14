using SportsEventApp.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsEventApp.Grains.States
{
    public class SportEventState
    {
        public SportEventDto? EventInfo { get; set; }
        public List<string> RegisteredUsers { get; set; } = new();
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public bool IsUsersLoaded { get; set; } = false; 
    }
}
