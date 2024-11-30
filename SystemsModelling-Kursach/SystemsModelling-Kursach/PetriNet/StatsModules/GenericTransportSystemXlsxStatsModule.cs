using OfficeOpenXml;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules
{
    public class GenericTransportSystemXlsxStatsModule : IStatsModule
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
        private readonly decimal _stepTimeTreshold;
        private decimal _timeElapsed;
        private Dictionary<string, Place> _places;
        private List<(decimal,decimal, decimal, decimal, decimal, decimal, decimal, decimal, decimal)> _statsStory;
        public decimal MeanQueueE1 => _meanQueueE1;
        public decimal MeanQueueE2 => _meanQueueE2;
        public decimal MeanQueueE3 => _meanQueueE3;
        public decimal MeanQueueG => _meanQueueG;

        public decimal OccupationE1 => _occupationE1;
        public decimal OccupationE2 => _occupationE2;
        public decimal OccupationE3 => _occupationE3;
        public decimal OccupationG => _occupationG;

        public GenericTransportSystemXlsxStatsModule(Dictionary<string, Place> places, decimal stepTimeTreshold, decimal skipTime = 0)
        {
            _meanQueueE1 = 0;
            _meanQueueE2 = 0;
            _meanQueueE3 = 0;
            _meanQueueG = 0;

            _occupationE1 = 0;
            _occupationE2 = 0;
            _occupationE3 = 0;
            _occupationG = 0;

            _places = places;
            _stepTimeTreshold = stepTimeTreshold;
            _lastTimeDoneStats = 0;
            _timeElapsed = skipTime;
            _statsStory = new();
        }

        public void DoStats(decimal timeCurrent)
        {
            var delta = timeCurrent - _lastTimeDoneStats;

            _meanQueueE1 += (_places["B1_F"].MarkerCount + _places["S1_F"].MarkerCount) * delta;
            _meanQueueE2 += (_places["B2_F"].MarkerCount + _places["S2_F"].MarkerCount) * delta;
            _meanQueueE3 += (_places["B3_F"].MarkerCount + _places["S3_F"].MarkerCount) * delta;
            _meanQueueG += _places
                            .Where(kv => Regex.IsMatch(kv.Key, @"^(S|B)([1-3])_\1Q[1-3]$"))
                            .Sum(kv => kv.Value.MarkerCount) * delta;
            _occupationE1 += (1 - _places["E1_F"].MarkerCount) * delta;
            _occupationE2 += (1 - _places["E2_F"].MarkerCount) * delta;
            _occupationE3 += (1 - _places["E3_F"].MarkerCount) * delta;
            _occupationG += (1 - _places["G_F"].MarkerCount) * delta;

            _lastTimeDoneStats = timeCurrent;
            _timeElapsed += delta;

            if(timeCurrent % _stepTimeTreshold == 0 )
            {
                _statsStory.Add(
                    (timeCurrent,
                    _meanQueueE1/ _timeElapsed,
                    _meanQueueE2 / _timeElapsed,
                    _meanQueueE3 / _timeElapsed,
                    _meanQueueG / _timeElapsed,
                    _occupationE1 / _timeElapsed,
                    _occupationE2 / _timeElapsed,
                    _occupationE3 / _timeElapsed,
                    _occupationG / _timeElapsed));
            }

        }

        public void PrintStats()
        {
            Console.WriteLine($"QE_1={_meanQueueE1 / _timeElapsed:0.00000}");
            Console.WriteLine($"QE_2={_meanQueueE2 / _timeElapsed:0.00000}");
            Console.WriteLine($"QE_3={_meanQueueE3 / _timeElapsed:0.00000}");
            Console.WriteLine($"QG={_meanQueueG / _timeElapsed:0.00000}");

            Console.WriteLine($"\nOE_1={_occupationE1 / _timeElapsed:0.00000}");
            Console.WriteLine($"OE_2={_occupationE2 / _timeElapsed:0.00000}");
            Console.WriteLine($"OE_3={_occupationE3 / _timeElapsed:0.00000}");
            Console.WriteLine($"OG={_occupationG / _timeElapsed:0.00000}");
        }

        public void ToXlsx(string path="")
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Testing results");

                worksheet.Cells[1, 1].Value = "Time";
                worksheet.Cells[1, 2].Value = "QE_1";
                worksheet.Cells[1, 3].Value = "QE_2";
                worksheet.Cells[1, 4].Value = "QE_3";
                worksheet.Cells[1, 5].Value = "QG";

                worksheet.Cells[1, 6].Value = "OE_1";
                worksheet.Cells[1, 7].Value = "OE_2";
                worksheet.Cells[1, 8].Value = "OE_3";
                worksheet.Cells[1, 9].Value = "OG";

                for (int i = 0; i < _statsStory.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = _statsStory[i].Item1;
                    worksheet.Cells[i + 2, 2].Value = _statsStory[i].Item2;
                    worksheet.Cells[i + 2, 3].Value = _statsStory[i].Item3;
                    worksheet.Cells[i + 2, 4].Value = _statsStory[i].Item4;
                    worksheet.Cells[i + 2, 5].Value = _statsStory[i].Item5;
                    worksheet.Cells[i + 2, 6].Value = _statsStory[i].Item6;
                    worksheet.Cells[i + 2, 7].Value = _statsStory[i].Item7;
                    worksheet.Cells[i + 2, 8].Value = _statsStory[i].Item8;
                    worksheet.Cells[i + 2, 9].Value = _statsStory[i].Item9;
                }

                File.WriteAllBytes(path, package.GetAsByteArray());
            }
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
