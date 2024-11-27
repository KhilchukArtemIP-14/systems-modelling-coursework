using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.DelayGenerators
{
    public class ConstantDelayGenerator:IDelayGenerator
    {
        private decimal _value;

        public ConstantDelayGenerator(decimal value)
        {
            _value = value;
        }

        public decimal GenerateDelay() => _value;
    }
}
