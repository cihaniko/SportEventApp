using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsEventApp.Infrastructure.DataAccess.Models
{
    public class SportEvent
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime StartTime { get; set; }
        public int Capacity { get; set; }
    }
}
