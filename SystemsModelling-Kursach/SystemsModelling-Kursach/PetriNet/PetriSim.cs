using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet
{
    public class PetriSim
    {
        private List<Transition> _transitions;
        private decimal _timeCurrent;
        private IStatsModule _statsModule;
        public PetriSim(List<Transition> transitions, IStatsModule statsModule = null)
        {
            _transitions = transitions;
            _timeCurrent = 0;
            _statsModule = statsModule;
        }

        public void Simulate(decimal simulationTime, decimal skiptime=0)
        {
            var random = new Random();
            while (_timeCurrent < simulationTime)
            {
                if (_transitions.All(t => !t.CanTrigger && t.Buffer == 0))
                {
                    _timeCurrent = simulationTime;
                    break;
                }

                var dueActOut = _transitions
                    .Where(t => t.MinTimeOut == _timeCurrent)
                    .OrderBy(_ => random.NextDouble())
                    .FirstOrDefault();

                while (dueActOut != null)
                {
                    dueActOut.ActOut();

                    dueActOut = _transitions
                        .Where(t => t.MinTimeOut == _timeCurrent)
                        .OrderBy(_ => random.NextDouble())
                        .FirstOrDefault();
                }

                var activeT = _transitions
                    .Where(t => t.CanTrigger) //active only
                    .OrderByDescending(t => t.Priority)// bigger priority first
                    .ThenBy(_=>random.NextDouble()) // randomize
                    .FirstOrDefault(); // first element or null if empty

                while (activeT != null)
                {
                    activeT.ActIn(_timeCurrent);

                    activeT = _transitions
                        .Where(t => t.CanTrigger)
                        .OrderByDescending(t => t.Priority)
                        .ThenBy(_ => random.NextDouble())
                        .FirstOrDefault();
                }

                _timeCurrent = _transitions.Min(t=>t.MinTimeOut);

                if(_timeCurrent>skiptime) _statsModule?.DoStats(_timeCurrent);
            }
        }
    }
}
