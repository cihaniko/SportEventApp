using SportsEventApp.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsEventApp.Grains.Grains
{
    [GrainType("PingGrain")]
    public class PingGrain : Grain, IPingGrain
    {
        public Task Ping() => Task.CompletedTask;
    }
}
