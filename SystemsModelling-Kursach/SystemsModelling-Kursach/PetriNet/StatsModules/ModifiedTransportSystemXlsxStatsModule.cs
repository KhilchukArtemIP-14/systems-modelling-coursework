using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules
{
    public class ModifiedTransportSystemXlsxStatsModule:IStatsModule
    {
        private decimal _meanQueueE;
        private decimal _meanQueueG;

        private decimal _occupationE;
        private decimal _occupationG;

        private decimal _lastTimeDoneStats;
        private decimal _timeElapsed;
        private Dictionary<string, Place> _places;
        private readonly decimal _stepTimeTreshold;

        public decimal MeanQueueE => _meanQueueE;
        public decimal MeanQueueG => _meanQueueG;

        public decimal OccupationE => _occupationE;
        public decimal OccupationG => _occupationG;

        private List<(decimal, decimal, decimal, decimal, decimal)> _statsStory;


        public ModifiedTransportSystemXlsxStatsModule(Dictionary<string, Place> places, decimal stepTimeTreshold, decimal skipTime = 0)
        {
            _meanQueueE = 0;
            _meanQueueG = 0;

            _occupationE = 0;
            _occupationG = 0;

            _places = places;
            _stepTimeTreshold = stepTimeTreshold;
            _lastTimeDoneStats = skipTime;

            _statsStory = new();
        }

        public void DoStats(decimal timeCurrent)
        {
            var delta = timeCurrent - _lastTimeDoneStats;

            _meanQueueE += _places
                            .Where(kv => Regex.IsMatch(kv.Key, @"^(S|B)F_Q[1-6]$"))
                            .Select(kv => kv.Value.MarkerCount).Sum() * delta;
            _meanQueueG += (_places["SQ"].MarkerCount + _places["BQ"].MarkerCount) * delta;

            _occupationE += (3 - _places["E_F"].MarkerCount) / 3m * delta;
            _occupationG += (1 - _places["G_F"].MarkerCount) * delta;

            _lastTimeDoneStats = timeCurrent;
            _timeElapsed += delta;

            if(timeCurrent% _stepTimeTreshold == 0)
            {
                _statsStory.Add(
                    (timeCurrent,
                    _meanQueueE / _timeElapsed,
                    _meanQueueG / _timeElapsed,
                    _occupationE / _timeElapsed,
                    _occupationG / _timeElapsed));
            }
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
        public void ToXlsx(string path = "")
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Testing results");

                worksheet.Cells[1, 1].Value = "Time";
                worksheet.Cells[1, 2].Value = "QE";
                worksheet.Cells[1, 3].Value = "QG";

                worksheet.Cells[1, 4].Value = "OE";
                worksheet.Cells[1, 5].Value = "OG";

                for (int i = 0; i < _statsStory.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = _statsStory[i].Item1;
                    worksheet.Cells[i + 2, 2].Value = _statsStory[i].Item2;
                    worksheet.Cells[i + 2, 3].Value = _statsStory[i].Item3;
                    worksheet.Cells[i + 2, 4].Value = _statsStory[i].Item4;
                    worksheet.Cells[i + 2, 5].Value = _statsStory[i].Item5;
                }

                File.WriteAllBytes(path, package.GetAsByteArray());
            }
        }
    }
}
