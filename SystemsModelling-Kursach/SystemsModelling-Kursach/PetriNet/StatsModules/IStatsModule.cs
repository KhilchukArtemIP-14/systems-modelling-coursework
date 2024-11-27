using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules
{
    public interface IStatsModule
    {
        public void DoStats(decimal timeCurrent);
        public void PrintStats();
        public Dictionary<string, decimal> GetStats();
    }
}
