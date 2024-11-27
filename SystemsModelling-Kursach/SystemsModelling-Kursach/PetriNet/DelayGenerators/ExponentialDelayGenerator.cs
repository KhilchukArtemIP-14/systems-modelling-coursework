using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.DelayGenerators
{
    public class ExponentialDelayGenerator:IDelayGenerator
    {
        private decimal _lambda;
        private Random _random;
        public ExponentialDelayGenerator(decimal lambda)
        {
            _lambda = lambda;
            _random = new Random();
        }
        public decimal GenerateDelay()
        {
            return -(decimal)Math.Log(_random.NextDouble()) / _lambda;
        }
    }
}
