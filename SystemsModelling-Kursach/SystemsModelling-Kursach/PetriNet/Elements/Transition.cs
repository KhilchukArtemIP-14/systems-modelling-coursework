using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.DelayGenerators;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements
{
    public class Transition
    {
        private string _name;
        private int _priority;
        private IDelayGenerator _generator;
        private List<decimal> _timeOuts;
        private List<Place> _inPlaces;
        private List<Place> _outPlaces;

        public decimal MinTimeOut=> _timeOuts.Any() ? _timeOuts.Min() : Decimal.MaxValue;
        public string Name => _name;
        public int Priority => _priority;
        public int Buffer => _timeOuts.Count;
        public bool CanTrigger => _inPlaces.All(p => p.MarkerCount != 0);

        public Transition(
            string name,
            IDelayGenerator generator,
            List<Place> inPlaces,
            List<Place> outPlaces,
            int priority = 0)
        {
            _name = name;
            _generator = generator;
            _inPlaces = inPlaces;
            _outPlaces = outPlaces;
            _timeOuts = new();
            _priority = priority;
        }

        public void ActIn(decimal timeCurrent) 
        {
            if (CanTrigger)
            {
                foreach (var position in _inPlaces)
                {
                    position.DecreaseMark();
                }

                _timeOuts.Add(timeCurrent + _generator.GenerateDelay());
            }
            else throw new ArgumentException("Doesn't have markers in input positions");
        }
        public void ActOut() 
        {
            if (Buffer != 0)
            {
                foreach(var position in _outPlaces)
                {
                    position.IncreaseMark();
                }

                _timeOuts.Remove(_timeOuts.Min());
            }
            else throw new ArgumentException("Doesn't have markers to release");
        }
    }
}
