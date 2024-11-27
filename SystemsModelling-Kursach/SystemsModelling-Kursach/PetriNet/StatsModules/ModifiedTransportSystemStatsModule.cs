using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;
namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules
{
    public class ModifiedTransportSystemStatsModule : IStatsModule
    {
        private decimal _meanQueueE;
        private decimal _meanQueueG;

        private decimal _occupationE;
        private decimal _occupationG;

        private decimal _lastTimeDoneStats;
        private decimal _timeElapsed;
        private Dictionary<string, Position> _positions;

        public decimal MeanQueueE => _meanQueueE;
        public decimal MeanQueueG => _meanQueueG;

        public decimal OccupationE => _occupationE;
        public decimal OccupationG => _occupationG;

        public ModifiedTransportSystemStatsModule(Dictionary<string, Position> positions, decimal skipTime = 0)
        {
            _meanQueueE = 0;
            _meanQueueG = 0;

            _occupationE = 0;
            _occupationG = 0;

            _positions = positions;
            _lastTimeDoneStats = skipTime;
        }

        public void DoStats(decimal timeCurrent)
        {
            var delta = timeCurrent - _lastTimeDoneStats;
            
            _meanQueueE += _positions
                            .Where(kv => Regex.IsMatch(kv.Key, @"^(S|B)F_Q[1-6]$"))
                            .Select(kv => kv.Value.MarkerCount).Sum()*delta;
            _meanQueueG += (_positions["SQ"].MarkerCount + _positions["BQ"].MarkerCount) * delta;

            _occupationE += (3 - _positions["E_F"].MarkerCount) / 3m * delta;
            _occupationG += (1 - _positions["G_F"].MarkerCount) * delta;

            _lastTimeDoneStats = timeCurrent;
            _timeElapsed += delta;
        }

        public void PrintStats()
        {
            Console.WriteLine($"QE={_meanQueueE / _timeElapsed:0.00000}");
            Console.WriteLine($"QG={_meanQueueG / _timeElapsed:0.00000}");

            Console.WriteLine($"\nOE={_occupationE / _timeElapsed:0.00000}");
            Console.WriteLine($"OG={_occupationG / _timeElapsed:0.00000}");
        }

        public Dictionary<string, decimal> GetStats()
        {
            return new Dictionary<string, decimal>()
            {
                { "QE", _meanQueueE/ _timeElapsed},
                { "QG", _meanQueueG/ _timeElapsed},

                { "OE", _occupationE/ _timeElapsed},
                { "OG", _occupationG/ _timeElapsed},
            };
        }
    }
}
