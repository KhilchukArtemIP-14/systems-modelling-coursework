using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.DelayGenerators
{
    public interface IDelayGenerator
    {
        public decimal GenerateDelay();
    }
}
