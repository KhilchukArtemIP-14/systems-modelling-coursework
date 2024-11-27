using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules;

namespace SystemsModelling_Kursach.BuildingBlocks.Models
{
    public class PetriModel
    {
        public PetriSim PetriSim { get; set; }
        public Dictionary<string, Position> Positions { get; set; }
        public Dictionary<string, Transition> Transitions { get; set; }
        public IStatsModule StatsModule { get; set; }
    }
}
