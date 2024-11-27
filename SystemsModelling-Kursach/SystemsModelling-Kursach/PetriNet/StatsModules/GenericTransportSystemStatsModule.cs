using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules
{
    public class GenericTransportSystemStatsModule : IStatsModule
    {
        private decimal _meanQueueE1;
        private decimal _meanQueueE2;
        private decimal _meanQueueE3;
        private decimal _meanQueueG;

        private decimal _occupationE1;
        private decimal _occupationE2;
        private decimal _occupationE3;
        private decimal _occupationG;

        private decimal _lastTimeDoneStats;
        private decimal _timeElapsed;
        private Dictionary<string, Position> _positions;

        public decimal MeanQueueE1 => _meanQueueE1;
        public decimal MeanQueueE2 => _meanQueueE2;
        public decimal MeanQueueE3 => _meanQueueE3;
        public decimal MeanQueueG => _meanQueueG;

        public decimal OccupationE1 => _occupationE1;
        public decimal OccupationE2 => _occupationE2;
        public decimal OccupationE3 => _occupationE3;
        public decimal OccupationG => _occupationG;

        public GenericTransportSystemStatsModule(Dictionary<string, Position> positions, decimal skipTime=0)
        {
            _meanQueueE1 = 0;
            _meanQueueE2 = 0;
            _meanQueueE3 = 0;
            _meanQueueG = 0;

            _occupationE1 = 0;
            _occupationE2 = 0;
            _occupationE3 = 0;
            _occupationG = 0;

            _positions = positions;
            _lastTimeDoneStats = skipTime;
        }

        public void DoStats(decimal timeCurrent)
        {
            var delta = timeCurrent - _lastTimeDoneStats;

            _meanQueueE1 += (_positions["B1_F"].MarkerCount + _positions["S1_F"].MarkerCount) * delta;
            _meanQueueE2 += (_positions["B2_F"].MarkerCount + _positions["S2_F"].MarkerCount) * delta;
            _meanQueueE3 += (_positions["B3_F"].MarkerCount + _positions["S3_F"].MarkerCount) * delta;
            _meanQueueG += _positions
                            .Where(kv=>Regex.IsMatch(kv.Key, @"^(S|B)([1-3])_\1Q[1-3]$"))
                            .Sum(kv=>kv.Value.MarkerCount) * delta;

            _occupationE1 += (1 - _positions["E1_F"].MarkerCount) * delta;
            _occupationE2 += (1 - _positions["E2_F"].MarkerCount) * delta;
            _occupationE3 += (1 - _positions["E3_F"].MarkerCount) * delta;
            _occupationG += (1 - _positions["G_F"].MarkerCount) * delta;

            _lastTimeDoneStats = timeCurrent;
            _timeElapsed += delta;
        }

        public void PrintStats()
        {
            Console.WriteLine($"QE_1={_meanQueueE1/ _timeElapsed:0.00000}");
            Console.WriteLine($"QE_2={_meanQueueE2 / _timeElapsed:0.00000}");
            Console.WriteLine($"QE_3={_meanQueueE3 / _timeElapsed:0.00000}");
            Console.WriteLine($"QG={_meanQueueG / _timeElapsed:0.00000}");

            Console.WriteLine($"\nOE_1={_occupationE1 / _timeElapsed:0.00000}");
            Console.WriteLine($"OE_2={_occupationE2 / _timeElapsed:0.00000}");
            Console.WriteLine($"OE_3={_occupationE3 / _timeElapsed:0.00000}");
            Console.WriteLine($"OG={_occupationG / _timeElapsed:0.00000}");
        }

        public Dictionary<string, decimal> GetStats()
        {
            return new Dictionary<string, decimal>()
            {
                { "QE_1", _meanQueueE1/ _timeElapsed},
                { "QE_2", _meanQueueE2/ _timeElapsed},
                { "QE_3", _meanQueueE3/ _timeElapsed},
                { "QG", _meanQueueG/ _timeElapsed},

                { "OE_1", _occupationE1/ _timeElapsed},
                { "OE_2", _occupationE2/ _timeElapsed},
                { "OE_3", _occupationE3/ _timeElapsed},
                { "OG", _occupationG/ _timeElapsed},
            };
        }
    }
}
