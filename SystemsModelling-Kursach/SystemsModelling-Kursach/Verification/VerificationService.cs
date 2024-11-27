using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.Factories;
using SystemsModelling_Kursach.BuildingBlocks.Models;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules;

namespace SystemsModelling_Kursach.Verification
{
    public class VerificationService
    {
        private Func<decimal, decimal, decimal, decimal, decimal, decimal, decimal, decimal, PetriModel> _modelGeneratorDelegate;
        public VerificationService()
            : this(PetriSimFactory.GenericTransportSystem)
        {
        }
        public VerificationService(Func<decimal, decimal, decimal, decimal, decimal, decimal, decimal, decimal, PetriModel> modelGenerator)
        {
            _modelGeneratorDelegate = modelGenerator;
        }

        public void Verify()
        {
            decimal simulationTime = 10_000;
            int runsCount = 20;

            Console.WriteLine("Proceeding with verification");

            decimal[] values = 
                [10, 4,
                5, 5,
                3, 2,
                2.5m, 1.5m];
            string[] paramNames = [
                "Big truck load mean",
                "Big truck unload mean",
                "Small truck load mean",
                "Small truck unload mean",
                "Big truck go to grinder time",
                "Big truck return from grinder time",
                "Small truck go to grinder time",
                "Small truck return from grinder time",
            ];

            Console.WriteLine("Baseline:");

            var stats = DoRuns(runsCount,values,simulationTime);

            var statistics = stats.Select(x => x.GetStats());

            foreach (var key in statistics.First().Keys)
            {
                var averageValue = statistics.Average(s => s[key]);
                Console.WriteLine($"{key}={averageValue:0.000}");
            }


            for (int i = 0; i < values.Length; i++)
            {
                decimal[] scaledValues = (decimal[])values.Clone();
                scaledValues[i] *= 4;
                Console.WriteLine($"\nQuadrupling parameter: {paramNames[i]}");

                stats = DoRuns(runsCount, scaledValues, simulationTime);

                statistics = stats.Select(x => x.GetStats());
                foreach (var key in statistics.First().Keys)
                {
                    var averageValue = statistics.Average(s => s[key]);
                    Console.WriteLine($"{key}={averageValue:0.000}");
                }
            }
        }

        private List<IStatsModule> DoRuns(int runCount, decimal[] parameters, decimal simulationTime)
        {
            var stats = new List<IStatsModule>();

            for (int j = 0; j < runCount; j++)
            {
                var model = _modelGeneratorDelegate(
                    parameters[0],
                    parameters[1],
                    parameters[2],
                    parameters[3],
                    parameters[4],
                    parameters[5],
                    parameters[6],
                    parameters[7]);

                model.PetriSim.Simulate(simulationTime);

                stats.Add(model.StatsModule);
            }

            return stats;
        }
    }
}
