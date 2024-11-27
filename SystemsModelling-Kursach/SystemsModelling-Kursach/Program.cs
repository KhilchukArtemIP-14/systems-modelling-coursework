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
            /*var position1 = new Position("P1", 1);
            var position2 = new Position("P2", 0);

            var transition = new Transition("T1",new ExponentialDelayGenerator(1m/5m), new() { position1}, new() { position1,position2 });

            var sim = new PetriSim(new() { transition });
            sim.Simulate(10000);
            Console.WriteLine(position2.MarkerCount);*/

            /*for(int i = 0;i<20;i++)
            {
                Console.WriteLine($"Simulation number {i+1}");
                var model = PetriSimFactory.GenericTransportSystem(
                    10, 4,
                    5, 5,
                    3, 2,
                    2.5m, 1.5m
                    );

                model.PetriSim.Simulate(100_000, 11250);

                model.StatsModule.PrintStats(400_000 - 11250);
            }*/

            /*var verificator = new VerificationService(PetriSimFactory.ModifiedTransportSystem);

            verificator.Verify();

            var yoho = PetriSimFactory.ModifiedTransportSystemIntervalStatsGather(750);

            yoho.PetriSim.Simulate(30000);

            var module = yoho.StatsModule as ModifiedTransportSystemXlsxStatsModule;

            module.ToXlsx(@"C:\Users\artem\OneDrive\Documents\KPI4_1\modelsys\kursach\data.xlsx");*/

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

            /*var verificator = new VerificationService();

            verificator.Verify();*/

            /*var yoho = PetriSimFactory.TransportSystemIntervalStatsGather(750);

            yoho.PetriSim.Simulate(30000);

            var module = yoho.StatsModule as TransportSystemXlsxStatsModule;

            module.ToXlsx(@"C:\Users\artem\OneDrive\Documents\KPI4_1\modelsys\kursach\data.xlsx");*/
        }
    }
}
