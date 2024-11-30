using SystemsModelling_Kursach.BuildingBlocks.Factories;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.DelayGenerators;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules;
using SystemsModelling_Kursach.Verification;

namespace SystemsModelling_Kursach
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var verificator = new VerificationService(PetriSimFactory.GenericTransportSystem);
            verificator.Verify();

            for (int i = 0;i<20;i++)
            {
                Console.WriteLine($"Simulation number {i+1}");
                var model = PetriSimFactory.GenericTransportSystem(
                    10, 4,
                    5, 5,
                    3, 2,
                    2.5m, 1.5m
                    );

                model.PetriSim.Simulate(100_000, 11250);

                model.StatsModule.PrintStats();
            }


            verificator = new VerificationService(PetriSimFactory.ModifiedTransportSystem);
            verificator.Verify();

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($"Simulation number {i + 1}");
                var model = PetriSimFactory.ModifiedTransportSystem(
                    10, 4,
                    5, 5,
                    3, 2,
                    2.5m, 1.5m
                    );

                model.PetriSim.Simulate(100_000, 17_250);

                model.StatsModule.PrintStats();
            }

            Console.WriteLine("For F1:");
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine($"Simulation number {i + 1}");
                var model = PetriSimFactory.GenericTransportSystem(
                    10, 4,
                    5, 5,
                    3, 2,
                    2.5m, 1.5m
                    );

                model.PetriSim.Simulate(100_000, 11250);

                Console.WriteLine($"{model.StatsModule.GetStats()["OG"]:0.00000}");
            }

            Console.WriteLine("For F2:");
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine($"Simulation number {i + 1}");
                var model = PetriSimFactory.ModifiedTransportSystem(
                    10, 4,
                    5, 5,
                    3, 2,
                    2.5m, 1.5m
                    );

                model.PetriSim.Simulate(100_000, 17_250);

                Console.WriteLine($"{model.StatsModule.GetStats()["OG"]:0.00000}");
            }
        }
    }
}
