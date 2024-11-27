using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsModelling_Kursach.BuildingBlocks.Models;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.DelayGenerators;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements;
using SystemsModelling_Kursach.BuildingBlocks.PetriNet.StatsModules;
using Position = SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements.Position;
using Transition = SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements.Transition;

namespace SystemsModelling_Kursach.BuildingBlocks.Factories
{
    public static class PetriSimFactory
    {
        public static PetriModel GenericTransportSystem(
            decimal loadBigTruckMeanTime,
            decimal unloadBigTruckMeanTime,
            decimal loadSmallTruckMeanTime,
            decimal unloadSmallTruckMeanTime,
            decimal goToGrinderBigTruckTime,
            decimal returnFromGrinderBigTruckTime,
            decimal goToGrinderSmallTruckTime,
            decimal returnFromGrinderSmallTruckTime
            )
        {
            var lambdaBigTruckLoad = 1m / loadBigTruckMeanTime;
            var lambdaBigTruckUnload = 1m / unloadBigTruckMeanTime;
            var lambdaSmallTruckLoad = 1m / loadSmallTruckMeanTime;
            var lambdaSmallTruckUnload = 1m / unloadSmallTruckMeanTime;

            var positions = new Dictionary<string, Position>
                {
                    { "E1_F", new Position("E1_F", 1) },
                    { "E2_F", new Position("E2_F", 1) },
                    { "E3_F", new Position("E3_F", 1) },

                    { "B1_F", new Position("B1_F", 1) },
                    { "B1_L", new Position("B1_L") },
                    { "B1_BQ1", new Position("B1_BQ1") },
                    { "B1_BQ2", new Position("B1_BQ2") },
                    { "B1_BQ3", new Position("B1_BQ3") },
                    { "B1_unloaded", new Position("B1_unloaded") },

                    { "B2_F", new Position("B2_F", 1) },
                    { "B2_L", new Position("B2_L") },
                    { "B2_BQ1", new Position("B2_BQ1") },
                    { "B2_BQ2", new Position("B2_BQ2") },
                    { "B2_BQ3", new Position("B2_BQ3") },
                    { "B2_unloaded", new Position("B2_unloaded") },

                    { "B3_F", new Position("B3_F", 1) },
                    { "B3_L", new Position("B3_L") },
                    { "B3_BQ1", new Position("B3_BQ1") },
                    { "B3_BQ2", new Position("B3_BQ2") },
                    { "B3_BQ3", new Position("B3_BQ3") },
                    { "B3_unloaded", new Position("B3_unloaded") },

                    { "BQ1_F", new Position("BQ1_F", 1) },
                    { "BQ2_F", new Position("BQ2_F", 1) },

                    { "S1_F", new Position("S1_F", 1) },
                    { "S1_L", new Position("S1_L") },
                    { "S1_SQ1", new Position("S1_SQ1") },
                    { "S1_SQ2", new Position("S1_SQ2") },
                    { "S1_SQ3", new Position("S1_SQ3") },
                    { "S1_unloaded", new Position("S1_unloaded") },

                    { "S2_F", new Position("S2_F", 1) },
                    { "S2_L", new Position("S2_L") },
                    { "S2_SQ1", new Position("S2_SQ1") },
                    { "S2_SQ2", new Position("S2_SQ2") },
                    { "S2_SQ3", new Position("S2_SQ3") },
                    { "S2_unloaded", new Position("S2_unloaded") },

                    { "S3_F", new Position("S3_F", 1) },
                    { "S3_L", new Position("S3_L") },
                    { "S3_SQ1", new Position("S3_SQ1") },
                    { "S3_SQ2", new Position("S3_SQ2") },
                    { "S3_SQ3", new Position("S3_SQ3") },
                    { "S3_unloaded", new Position("S3_unloaded") },

                    { "SQ1_F", new Position("SQ1_F", 1) },
                    { "SQ2_F", new Position("SQ2_F", 1) },

                    { "G_F", new Position("G_F", 1) }
                };

            var transitions = new Dictionary<string, Transition>
            {
                // transitions for B1
                {
                    "Load B1", new Transition(
                        "Load B1",
                        new ExponentialDelayGenerator(lambdaBigTruckLoad),
                        new List<Position> { positions["E1_F"], positions["B1_F"] },
                        new List<Position> { positions["B1_L"], positions["E1_F"] }
                    )
                },
                {
                    "Go to grinder B1", new Transition(
                        "Go to grinder B1",
                        new ConstantDelayGenerator(goToGrinderBigTruckTime),
                        new List<Position> { positions["B1_L"] },
                        new List<Position> { positions["B1_BQ3"] }
                    )
                },
                {
                    "Advance B1_BQ2", new Transition(
                        "Advance B1_BQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B1_BQ3"], positions["BQ2_F"] },
                        new List<Position> { positions["B1_BQ2"] }
                    )
                },
                {
                    "Advance B1_BQ1", new Transition(
                        "Advance B1_BQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B1_BQ2"], positions["BQ1_F"] },
                        new List<Position> { positions["B1_BQ1"], positions["BQ2_F"] }
                    )
                },
                {
                    "Unload B1", new Transition(
                        "Unload B1",
                        new ExponentialDelayGenerator(lambdaBigTruckUnload),
                        new List<Position> { positions["B1_BQ1"], positions["G_F"] },
                        new List<Position> { positions["B1_unloaded"], positions["BQ1_F"], positions["G_F"] },
                        1
                    )
                },
                {
                    "Go back B1", new Transition(
                        "Go back B1",
                        new ConstantDelayGenerator(returnFromGrinderBigTruckTime),
                        new List<Position> { positions["B1_unloaded"] },
                        new List<Position> { positions["B1_F"] }
                    )
                },

                // transitions for B2
                {
                    "Load B2", new Transition(
                        "Load B2",
                        new ExponentialDelayGenerator(lambdaBigTruckLoad),
                        new List<Position> { positions["E2_F"], positions["B2_F"] },
                        new List<Position> { positions["B2_L"], positions["E2_F"] }
                    )
                },
                {
                    "Go to grinder B2", new Transition(
                        "Go to grinder B2",
                        new ConstantDelayGenerator(goToGrinderBigTruckTime),
                        new List<Position> { positions["B2_L"] },
                        new List<Position> { positions["B2_BQ3"] }
                    )
                },
                {
                    "Advance B2_BQ2", new Transition(
                        "Advance B2_BQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B2_BQ3"], positions["BQ2_F"] },
                        new List<Position> { positions["B2_BQ2"] }
                    )
                },
                {
                    "Advance B2_BQ1", new Transition(
                        "Advance B2_BQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B2_BQ2"], positions["BQ1_F"] },
                        new List<Position> { positions["B2_BQ1"], positions["BQ2_F"] }
                    )
                },
                {
                    "Unload B2", new Transition(
                        "Unload B2",
                        new ExponentialDelayGenerator(lambdaBigTruckUnload),
                        new List<Position> { positions["B2_BQ1"], positions["G_F"] },
                        new List<Position> { positions["B2_unloaded"], positions["BQ1_F"], positions["G_F"] },
                        1
                    )
                },
                {
                    "Go back B2", new Transition(
                        "Go back B2",
                        new ConstantDelayGenerator(returnFromGrinderBigTruckTime),
                        new List<Position> { positions["B2_unloaded"] },
                        new List<Position> { positions["B2_F"] }
                    )
                },

                // transitions for B3
                {
                    "Load B3", new Transition(
                        "Load B3",
                        new ExponentialDelayGenerator(lambdaBigTruckLoad),
                        new List<Position> { positions["E3_F"], positions["B3_F"] },
                        new List<Position> { positions["B3_L"], positions["E3_F"] }
                    )
                },
                {
                    "Go to grinder B3", new Transition(
                        "Go to grinder B3",
                        new ConstantDelayGenerator(goToGrinderBigTruckTime),
                        new List<Position> { positions["B3_L"] },
                        new List<Position> { positions["B3_BQ3"] }
                    )
                },
                {
                    "Advance B3_BQ2", new Transition(
                        "Advance B3_BQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B3_BQ3"], positions["BQ2_F"] },
                        new List<Position> { positions["B3_BQ2"] }
                    )
                },
                {
                    "Advance B3_BQ1", new Transition(
                        "Advance B3_BQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B3_BQ2"], positions["BQ1_F"] },
                        new List<Position> { positions["B3_BQ1"], positions["BQ2_F"] }
                    )
                },
                {
                    "Unload B3", new Transition(
                        "Unload B3",
                        new ExponentialDelayGenerator(lambdaBigTruckUnload),
                        new List<Position> { positions["B3_BQ1"], positions["G_F"] },
                        new List<Position> { positions["B3_unloaded"], positions["BQ1_F"], positions["G_F"] },
                        1
                    )
                },
                {
                    "Go back B3", new Transition(
                        "Go back B3",
                        new ConstantDelayGenerator(returnFromGrinderBigTruckTime),
                        new List<Position> { positions["B3_unloaded"] },
                        new List<Position> { positions["B3_F"] }
                    )
                },

                // transitions for S1
                {
                    "Load S1", new Transition(
                        "Load S1",
                        new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                        new List<Position> { positions["E1_F"], positions["S1_F"] },
                        new List<Position> { positions["S1_L"], positions["E1_F"] }
                    )
                },
                {
                    "Go to grinder S1", new Transition(
                        "Go to grinder S1",
                        new ConstantDelayGenerator(goToGrinderSmallTruckTime),
                        new List<Position> { positions["S1_L"] },
                        new List<Position> { positions["S1_SQ3"] }
                    )
                },
                {
                    "Advance S1_SQ2", new Transition(
                        "Advance S1_SQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S1_SQ3"], positions["SQ2_F"] },
                        new List<Position> { positions["S1_SQ2"] }
                    )
                },
                {
                    "Advance S1_SQ1", new Transition(
                        "Advance S1_SQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S1_SQ2"], positions["SQ1_F"] },
                        new List<Position> { positions["S1_SQ1"], positions["SQ2_F"] }
                    )
                },
                {
                    "Unload S1", new Transition(
                        "Unload S1",
                        new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                        new List<Position> { positions["S1_SQ1"], positions["G_F"] },
                        new List<Position> { positions["S1_unloaded"], positions["SQ1_F"], positions["G_F"] }
                    )
                },
                {
                    "Go back S1", new Transition(
                        "Go back S1",
                        new ConstantDelayGenerator(returnFromGrinderSmallTruckTime),
                        new List<Position> { positions["S1_unloaded"] },
                        new List<Position> { positions["S1_F"] }
                    )
                },

                // transitions for S2
                {
                    "Load S2", new Transition(
                        "Load S2",
                        new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                        new List<Position> { positions["E2_F"], positions["S2_F"] },
                        new List<Position> { positions["S2_L"], positions["E2_F"] }
                    )
                },
                {
                    "Go to grinder S2", new Transition(
                        "Go to grinder S2",
                        new ConstantDelayGenerator(goToGrinderSmallTruckTime),
                        new List<Position> { positions["S2_L"] },
                        new List<Position> { positions["S2_SQ3"] }
                    )
                },
                {
                    "Advance S2_SQ2", new Transition(
                        "Advance S2_SQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S2_SQ3"], positions["SQ2_F"] },
                        new List<Position> { positions["S2_SQ2"] }
                    )
                },
                {
                    "Advance S2_SQ1", new Transition(
                        "Advance S2_SQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S2_SQ2"], positions["SQ1_F"] },
                        new List<Position> { positions["S2_SQ1"], positions["SQ2_F"] }
                    )
                },
                {
                    "Unload S2", new Transition(
                        "Unload S2",
                        new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                        new List<Position> { positions["S2_SQ1"], positions["G_F"] },
                        new List<Position> { positions["S2_unloaded"], positions["SQ1_F"], positions["G_F"] }
                    )
                },
                {
                    "Go back S2", new Transition(
                        "Go back S2",
                        new ConstantDelayGenerator(returnFromGrinderSmallTruckTime),
                        new List<Position> { positions["S2_unloaded"] },
                        new List<Position> { positions["S2_F"] }
                    )
                },

                // transitions for S3
                {
                    "Load S3", new Transition(
                        "Load S3",
                        new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                        new List<Position> { positions["E3_F"], positions["S3_F"] },
                        new List<Position> { positions["S3_L"], positions["E3_F"] }
                    )
                },
                {
                    "Go to grinder S3", new Transition(
                        "Go to grinder S3",
                        new ConstantDelayGenerator(goToGrinderSmallTruckTime),
                        new List<Position> { positions["S3_L"] },
                        new List<Position> { positions["S3_SQ3"] }
                    )
                },
                {
                    "Advance S3_SQ2", new Transition(
                        "Advance S3_SQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S3_SQ3"], positions["SQ2_F"] },
                        new List<Position> { positions["S3_SQ2"] }
                    )
                },
                {
                    "Advance S3_SQ1", new Transition(
                        "Advance S3_SQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S3_SQ2"], positions["SQ1_F"] },
                        new List<Position> { positions["S3_SQ1"], positions["SQ2_F"] }
                    )
                },
                {
                    "Unload S3", new Transition(
                        "Unload S3",
                        new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                        new List<Position> { positions["S3_SQ1"], positions["G_F"] },
                        new List<Position> { positions["S3_unloaded"], positions["SQ1_F"], positions["G_F"] }
                    )
                },
                {
                    "Go back S3", new Transition(
                        "Go back S3",
                        new ConstantDelayGenerator(returnFromGrinderSmallTruckTime),
                        new List<Position> { positions["S3_unloaded"] },
                        new List<Position> { positions["S3_F"] }
                    )
                }
            };

            var statsModule = new GenericTransportSystemStatsModule(positions);

            return new PetriModel()
            {
                Positions = positions,
                Transitions = transitions,
                PetriSim = new PetriSim(transitions.Select(kv=>kv.Value).ToList(), statsModule),
                StatsModule = statsModule
            };
        }

        public static PetriModel GenericTransportSystemIntervalStatsGather(decimal interval)
        {
            decimal goToGrinderBigTruckTime = 3;
            decimal returnFromGrinderBigTruckTime = 2;
            decimal goToGrinderSmallTruckTime = 2.5m;
            decimal returnFromGrinderSmallTruckTime = 1.5m;

            var lambdaBigTruckLoad = 1m / 10;
            var lambdaBigTruckUnload = 1m / 4;
            var lambdaSmallTruckLoad = 1m / 5;
            var lambdaSmallTruckUnload = 1m / 5;

            var positions = new Dictionary<string, Position>
                {
                    { "Do_stats", new Position("Do_stats", 1) },

                    { "E1_F", new Position("E1_F", 1) },
                    { "E2_F", new Position("E2_F", 1) },
                    { "E3_F", new Position("E3_F", 1) },

                    { "B1_F", new Position("B1_F", 1) },
                    { "B1_L", new Position("B1_L") },
                    { "B1_BQ1", new Position("B1_BQ1") },
                    { "B1_BQ2", new Position("B1_BQ2") },
                    { "B1_BQ3", new Position("B1_BQ3") },
                    { "B1_unloaded", new Position("B1_unloaded") },

                    { "B2_F", new Position("B2_F", 1) },
                    { "B2_L", new Position("B2_L") },
                    { "B2_BQ1", new Position("B2_BQ1") },
                    { "B2_BQ2", new Position("B2_BQ2") },
                    { "B2_BQ3", new Position("B2_BQ3") },
                    { "B2_unloaded", new Position("B2_unloaded") },

                    { "B3_F", new Position("B3_F", 1) },
                    { "B3_L", new Position("B3_L") },
                    { "B3_BQ1", new Position("B3_BQ1") },
                    { "B3_BQ2", new Position("B3_BQ2") },
                    { "B3_BQ3", new Position("B3_BQ3") },
                    { "B3_unloaded", new Position("B3_unloaded") },

                    { "BQ1_F", new Position("BQ1_F", 1) },
                    { "BQ2_F", new Position("BQ2_F", 1) },

                    { "S1_F", new Position("S1_F", 1) },
                    { "S1_L", new Position("S1_L") },
                    { "S1_SQ1", new Position("S1_SQ1") },
                    { "S1_SQ2", new Position("S1_SQ2") },
                    { "S1_SQ3", new Position("S1_SQ3") },
                    { "S1_unloaded", new Position("S1_unloaded") },

                    { "S2_F", new Position("S2_F", 1) },
                    { "S2_L", new Position("S2_L") },
                    { "S2_SQ1", new Position("S2_SQ1") },
                    { "S2_SQ2", new Position("S2_SQ2") },
                    { "S2_SQ3", new Position("S2_SQ3") },
                    { "S2_unloaded", new Position("S2_unloaded") },

                    { "S3_F", new Position("S3_F", 1) },
                    { "S3_L", new Position("S3_L") },
                    { "S3_SQ1", new Position("S3_SQ1") },
                    { "S3_SQ2", new Position("S3_SQ2") },
                    { "S3_SQ3", new Position("S3_SQ3") },
                    { "S3_unloaded", new Position("S3_unloaded") },

                    { "SQ1_F", new Position("SQ1_F", 1) },
                    { "SQ2_F", new Position("SQ2_F", 1) },

                    { "G_F", new Position("G_F", 1) }
                };

            var transitions = new Dictionary<string, Transition>
            {
                {
                    "Do_stats", new Transition(
                        "Do_stats",
                        new ConstantDelayGenerator(interval),
                        new List<Position> { positions["Do_stats"] },
                        new List<Position> { positions["Do_stats"] }
                    )
                },

                // transitions for B1
                {
                    "Load B1", new Transition(
                        "Load B1",
                        new ExponentialDelayGenerator(lambdaBigTruckLoad),
                        new List<Position> { positions["E1_F"], positions["B1_F"] },
                        new List<Position> { positions["B1_L"], positions["E1_F"] }
                    )
                },
                {
                    "Go to grinder B1", new Transition(
                        "Go to grinder B1",
                        new ConstantDelayGenerator(goToGrinderBigTruckTime),
                        new List<Position> { positions["B1_L"] },
                        new List<Position> { positions["B1_BQ3"] }
                    )
                },
                {
                    "Advance B1_BQ2", new Transition(
                        "Advance B1_BQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B1_BQ3"], positions["BQ2_F"] },
                        new List<Position> { positions["B1_BQ2"] }
                    )
                },
                {
                    "Advance B1_BQ1", new Transition(
                        "Advance B1_BQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B1_BQ2"], positions["BQ1_F"] },
                        new List<Position> { positions["B1_BQ1"], positions["BQ2_F"] }
                    )
                },
                {
                    "Unload B1", new Transition(
                        "Unload B1",
                        new ExponentialDelayGenerator(lambdaBigTruckUnload),
                        new List<Position> { positions["B1_BQ1"], positions["G_F"] },
                        new List<Position> { positions["B1_unloaded"], positions["BQ1_F"], positions["G_F"] },
                        1
                    )
                },
                {
                    "Go back B1", new Transition(
                        "Go back B1",
                        new ConstantDelayGenerator(returnFromGrinderBigTruckTime),
                        new List<Position> { positions["B1_unloaded"] },
                        new List<Position> { positions["B1_F"] }
                    )
                },

                // transitions for B2
                {
                    "Load B2", new Transition(
                        "Load B2",
                        new ExponentialDelayGenerator(lambdaBigTruckLoad),
                        new List<Position> { positions["E2_F"], positions["B2_F"] },
                        new List<Position> { positions["B2_L"], positions["E2_F"] }
                    )
                },
                {
                    "Go to grinder B2", new Transition(
                        "Go to grinder B2",
                        new ConstantDelayGenerator(goToGrinderBigTruckTime),
                        new List<Position> { positions["B2_L"] },
                        new List<Position> { positions["B2_BQ3"] }
                    )
                },
                {
                    "Advance B2_BQ2", new Transition(
                        "Advance B2_BQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B2_BQ3"], positions["BQ2_F"] },
                        new List<Position> { positions["B2_BQ2"] }
                    )
                },
                {
                    "Advance B2_BQ1", new Transition(
                        "Advance B2_BQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B2_BQ2"], positions["BQ1_F"] },
                        new List<Position> { positions["B2_BQ1"], positions["BQ2_F"] }
                    )
                },
                {
                    "Unload B2", new Transition(
                        "Unload B2",
                        new ExponentialDelayGenerator(lambdaBigTruckUnload),
                        new List<Position> { positions["B2_BQ1"], positions["G_F"] },
                        new List<Position> { positions["B2_unloaded"], positions["BQ1_F"], positions["G_F"] },
                        1
                    )
                },
                {
                    "Go back B2", new Transition(
                        "Go back B2",
                        new ConstantDelayGenerator(returnFromGrinderBigTruckTime),
                        new List<Position> { positions["B2_unloaded"] },
                        new List<Position> { positions["B2_F"] }
                    )
                },

                // transitions for B3
                {
                    "Load B3", new Transition(
                        "Load B3",
                        new ExponentialDelayGenerator(lambdaBigTruckLoad),
                        new List<Position> { positions["E3_F"], positions["B3_F"] },
                        new List<Position> { positions["B3_L"], positions["E3_F"] }
                    )
                },
                {
                    "Go to grinder B3", new Transition(
                        "Go to grinder B3",
                        new ConstantDelayGenerator(goToGrinderBigTruckTime),
                        new List<Position> { positions["B3_L"] },
                        new List<Position> { positions["B3_BQ3"] }
                    )
                },
                {
                    "Advance B3_BQ2", new Transition(
                        "Advance B3_BQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B3_BQ3"], positions["BQ2_F"] },
                        new List<Position> { positions["B3_BQ2"] }
                    )
                },
                {
                    "Advance B3_BQ1", new Transition(
                        "Advance B3_BQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["B3_BQ2"], positions["BQ1_F"] },
                        new List<Position> { positions["B3_BQ1"], positions["BQ2_F"] }
                    )
                },
                {
                    "Unload B3", new Transition(
                        "Unload B3",
                        new ExponentialDelayGenerator(lambdaBigTruckUnload),
                        new List<Position> { positions["B3_BQ1"], positions["G_F"] },
                        new List<Position> { positions["B3_unloaded"], positions["BQ1_F"], positions["G_F"] },
                        1
                    )
                },
                {
                    "Go back B3", new Transition(
                        "Go back B3",
                        new ConstantDelayGenerator(returnFromGrinderBigTruckTime),
                        new List<Position> { positions["B3_unloaded"] },
                        new List<Position> { positions["B3_F"] }
                    )
                },

                // transitions for S1
                {
                    "Load S1", new Transition(
                        "Load S1",
                        new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                        new List<Position> { positions["E1_F"], positions["S1_F"] },
                        new List<Position> { positions["S1_L"], positions["E1_F"] }
                    )
                },
                {
                    "Go to grinder S1", new Transition(
                        "Go to grinder S1",
                        new ConstantDelayGenerator(goToGrinderSmallTruckTime),
                        new List<Position> { positions["S1_L"] },
                        new List<Position> { positions["S1_SQ3"] }
                    )
                },
                {
                    "Advance S1_SQ2", new Transition(
                        "Advance S1_SQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S1_SQ3"], positions["SQ2_F"] },
                        new List<Position> { positions["S1_SQ2"] }
                    )
                },
                {
                    "Advance S1_SQ1", new Transition(
                        "Advance S1_SQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S1_SQ2"], positions["SQ1_F"] },
                        new List<Position> { positions["S1_SQ1"], positions["SQ2_F"] }
                    )
                },
                {
                    "Unload S1", new Transition(
                        "Unload S1",
                        new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                        new List<Position> { positions["S1_SQ1"], positions["G_F"] },
                        new List<Position> { positions["S1_unloaded"], positions["SQ1_F"], positions["G_F"] }
                    )
                },
                {
                    "Go back S1", new Transition(
                        "Go back S1",
                        new ConstantDelayGenerator(returnFromGrinderSmallTruckTime),
                        new List<Position> { positions["S1_unloaded"] },
                        new List<Position> { positions["S1_F"] }
                    )
                },

                // transitions for S2
                {
                    "Load S2", new Transition(
                        "Load S2",
                        new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                        new List<Position> { positions["E2_F"], positions["S2_F"] },
                        new List<Position> { positions["S2_L"], positions["E2_F"] }
                    )
                },
                {
                    "Go to grinder S2", new Transition(
                        "Go to grinder S2",
                        new ConstantDelayGenerator(goToGrinderSmallTruckTime),
                        new List<Position> { positions["S2_L"] },
                        new List<Position> { positions["S2_SQ3"] }
                    )
                },
                {
                    "Advance S2_SQ2", new Transition(
                        "Advance S2_SQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S2_SQ3"], positions["SQ2_F"] },
                        new List<Position> { positions["S2_SQ2"] }
                    )
                },
                {
                    "Advance S2_SQ1", new Transition(
                        "Advance S2_SQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S2_SQ2"], positions["SQ1_F"] },
                        new List<Position> { positions["S2_SQ1"], positions["SQ2_F"] }
                    )
                },
                {
                    "Unload S2", new Transition(
                        "Unload S2",
                        new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                        new List<Position> { positions["S2_SQ1"], positions["G_F"] },
                        new List<Position> { positions["S2_unloaded"], positions["SQ1_F"], positions["G_F"] }
                    )
                },
                {
                    "Go back S2", new Transition(
                        "Go back S2",
                        new ConstantDelayGenerator(returnFromGrinderSmallTruckTime),
                        new List<Position> { positions["S2_unloaded"] },
                        new List<Position> { positions["S2_F"] }
                    )
                },

                // transitions for S3
                {
                    "Load S3", new Transition(
                        "Load S3",
                        new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                        new List<Position> { positions["E3_F"], positions["S3_F"] },
                        new List<Position> { positions["S3_L"], positions["E3_F"] }
                    )
                },
                {
                    "Go to grinder S3", new Transition(
                        "Go to grinder S3",
                        new ConstantDelayGenerator(goToGrinderSmallTruckTime),
                        new List<Position> { positions["S3_L"] },
                        new List<Position> { positions["S3_SQ3"] }
                    )
                },
                {
                    "Advance S3_SQ2", new Transition(
                        "Advance S3_SQ2",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S3_SQ3"], positions["SQ2_F"] },
                        new List<Position> { positions["S3_SQ2"] }
                    )
                },
                {
                    "Advance S3_SQ1", new Transition(
                        "Advance S3_SQ1",
                        new ConstantDelayGenerator(0),
                        new List<Position> { positions["S3_SQ2"], positions["SQ1_F"] },
                        new List<Position> { positions["S3_SQ1"], positions["SQ2_F"] }
                    )
                },
                {
                    "Unload S3", new Transition(
                        "Unload S3",
                        new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                        new List<Position> { positions["S3_SQ1"], positions["G_F"] },
                        new List<Position> { positions["S3_unloaded"], positions["SQ1_F"], positions["G_F"] }
                    )
                },
                {
                    "Go back S3", new Transition(
                        "Go back S3",
                        new ConstantDelayGenerator(returnFromGrinderSmallTruckTime),
                        new List<Position> { positions["S3_unloaded"] },
                        new List<Position> { positions["S3_F"] }
                    )
                }
            };

            var statsModule = new GenericTransportSystemXlsxStatsModule(positions,interval);

            return new PetriModel()
            {
                Positions = positions,
                Transitions = transitions,
                PetriSim = new PetriSim(transitions.Select(kv => kv.Value).ToList(), statsModule),
                StatsModule = statsModule
            };
        }

        public static PetriModel ModifiedTransportSystem(
            decimal loadBigTruckMeanTime,
            decimal unloadBigTruckMeanTime,
            decimal loadSmallTruckMeanTime,
            decimal unloadSmallTruckMeanTime,
            decimal goToGrinderBigTruckTime,
            decimal returnFromGrinderBigTruckTime,
            decimal goToGrinderSmallTruckTime,
            decimal returnFromGrinderSmallTruckTime
            )
        {
            var lambdaBigTruckLoad = 1m / loadBigTruckMeanTime;
            var lambdaBigTruckUnload = 1m / unloadBigTruckMeanTime;
            var lambdaSmallTruckLoad = 1m / loadSmallTruckMeanTime;
            var lambdaSmallTruckUnload = 1m / unloadSmallTruckMeanTime;

            var positions = new Dictionary<string, Position>
            {
                { "E_F", new Position("E_F", 3) },

                { "Q2_F", new Position("Q2_F", 1) },
                { "Q3_F", new Position("Q3_F", 1) },
                { "Q4_F", new Position("Q4_F", 1) },
                { "Q5_F", new Position("Q5_F", 1) },

                { "G_F", new Position("G_F", 1) },

                { "BF_Q1", new Position("BF_Q1") },
                { "BF_Q2", new Position("BF_Q2") },
                { "BF_Q3", new Position("BF_Q3") },
                { "BF_Q4", new Position("BF_Q4") },
                { "BF_Q5", new Position("BF_Q5") },
                { "BF_Q6", new Position("BF_Q6",3) },

                { "SF_Q1", new Position("SF_Q1") },
                { "SF_Q2", new Position("SF_Q2") },
                { "SF_Q3", new Position("SF_Q3") },
                { "SF_Q4", new Position("SF_Q4") },
                { "SF_Q5", new Position("SF_Q5") },
                { "SF_Q6", new Position("SF_Q6",3) },

                { "S_L", new Position("S_L") },
                { "B_L", new Position("B_L") },

                { "BQ", new Position("BQ") },
                { "SQ", new Position("SQ") },

                { "B unloaded", new Position("B unloaded") },
                { "S unloaded", new Position("S unloaded") },
            };

            var transitions = new Dictionary<string, Transition>
            {
                { "Load B", new Transition(
                    "Load B",
                    new ExponentialDelayGenerator(lambdaBigTruckLoad),
                    new List<Position> {  positions["BF_Q1"] },
                    new List<Position> { positions["B_L"],positions["E_F"] }) },
                { "Load S", new Transition(
                    "Load S",
                    new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                    new List<Position> { positions["SF_Q1"] },
                    new List<Position> { positions["S_L"], positions["E_F"]  }) },

                { "Advance BF_Q2", new Transition(
                    "Advance BF_Q2",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q2"], positions["E_F"], },
                    new List<Position> { positions["BF_Q1"], positions["Q2_F"] }) },
                { "Advance BF_Q3", new Transition(
                    "Advance BF_Q3",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q3"], positions["Q2_F"] },
                    new List<Position> { positions["BF_Q2"], positions["Q3_F"] }) },
                { "Advance BF_Q4", new Transition(
                    "Advance BF_Q4",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q4"], positions["Q3_F"] },
                    new List<Position> { positions["BF_Q3"], positions["Q4_F"] }) },
                { "Advance BF_Q5", new Transition(
                    "Advance BF_Q5",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q5"], positions["Q4_F"] },
                    new List<Position> { positions["BF_Q4"], positions["Q5_F"] }) },
                { "Advance BF_Q6", new Transition(
                    "Advance BF_Q6",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q6"], positions["Q5_F"] },
                    new List<Position> { positions["BF_Q5"] }) },

                { "Advance SF_Q2",new Transition(
                    "Advance SF_Q2",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q2"], positions["E_F"], },
                    new List<Position> { positions["SF_Q1"], positions["Q2_F"] }) },
                { "Advance SF_Q3", new Transition(
                    "Advance SF_Q3",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q3"], positions["Q2_F"] },
                    new List<Position> { positions["SF_Q2"], positions["Q3_F"] }) },
                { "Advance SF_Q4", new Transition(
                    "Advance SF_Q4",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q4"], positions["Q3_F"] },
                    new List<Position> { positions["SF_Q3"], positions["Q4_F"] }) },
                { "Advance SF_Q5", new Transition(
                    "Advance SF_Q5",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q5"], positions["Q4_F"] },
                    new List<Position> { positions["SF_Q4"], positions["Q5_F"] }) },
                { "Advance SF_Q6", new Transition(
                    "Advance SF_Q6",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q6"], positions["Q5_F"] },
                    new List<Position> { positions["SF_Q5"] }) },

                { "Go to grinder B_L", new Transition(
                    "Go to grinder B_L",
                    new ConstantDelayGenerator(goToGrinderBigTruckTime),
                    new List<Position> { positions["B_L"] },
                    new List<Position> { positions["BQ"] }) },
                { "Go to grinder S_L", new Transition(
                    "Go to grinder S_L",
                    new ConstantDelayGenerator(goToGrinderSmallTruckTime),
                    new List<Position> { positions["S_L"] },
                    new List<Position> { positions["SQ"] }) },

                { "Go back B", new Transition(
                    "Go back B",
                    new ConstantDelayGenerator(returnFromGrinderBigTruckTime),
                    new List<Position> {  positions["B unloaded"] },
                    new List<Position> {  positions["BF_Q6"],}) },
                { "Go back S", new Transition(
                    "Go back S",
                    new ConstantDelayGenerator(returnFromGrinderSmallTruckTime),
                    new List<Position> { positions["S unloaded"] },
                    new List<Position> { positions["SF_Q6"] }) },

                { "Unload B", new Transition(
                    "Unload B",
                    new ExponentialDelayGenerator(lambdaBigTruckUnload),
                    new List<Position> { positions["BQ"], positions["G_F"] },
                    new List<Position> { positions["B unloaded"], positions["G_F"] },
                    1) },
                { "Unload S",new Transition(
                    "Unload S",
                    new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                    new List<Position> { positions["SQ"], positions["G_F"] },
                    new List<Position> { positions["S unloaded"], positions["G_F"] }) }
            };

            var statsModule = new ModifiedTransportSystemStatsModule(positions);

            return new PetriModel()
            {
                Positions = positions,
                Transitions = transitions,
                PetriSim = new PetriSim(transitions.Select(kv => kv.Value).ToList(), statsModule),
                StatsModule = statsModule
            };
        }

        public static PetriModel ModifiedTransportSystemIntervalStatsGather(decimal interval)
        {
            var lambdaBigTruckLoad = 1m / 10;
            var lambdaBigTruckUnload = 1m / 4;
            var lambdaSmallTruckLoad = 1m / 5;
            var lambdaSmallTruckUnload = 1m / 5;

            var positions = new Dictionary<string, Position>
            {
                { "Do_stats", new Position("Do_stats", 1) },

                { "E_F", new Position("E_F", 3) },

                { "Q2_F", new Position("Q2_F", 1) },
                { "Q3_F", new Position("Q3_F", 1) },
                { "Q4_F", new Position("Q4_F", 1) },
                { "Q5_F", new Position("Q5_F", 1) },

                { "G_F", new Position("G_F", 1) },

                { "BF_Q1", new Position("BF_Q1") },
                { "BF_Q2", new Position("BF_Q2") },
                { "BF_Q3", new Position("BF_Q3") },
                { "BF_Q4", new Position("BF_Q4") },
                { "BF_Q5", new Position("BF_Q5") },
                { "BF_Q6", new Position("BF_Q6",3) },

                { "SF_Q1", new Position("SF_Q1") },
                { "SF_Q2", new Position("SF_Q2") },
                { "SF_Q3", new Position("SF_Q3") },
                { "SF_Q4", new Position("SF_Q4") },
                { "SF_Q5", new Position("SF_Q5") },
                { "SF_Q6", new Position("SF_Q6",3) },

                { "S_L", new Position("S_L") },
                { "B_L", new Position("B_L") },

                { "BQ", new Position("BQ") },
                { "SQ", new Position("SQ") },

                { "B unloaded", new Position("B unloaded") },
                { "S unloaded", new Position("S unloaded") },
            };

            var transitions = new Dictionary<string, Transition>
            {
                {
                    "Do_stats", new Transition(
                        "Do_stats",
                        new ConstantDelayGenerator(interval),
                        new List<Position> { positions["Do_stats"] },
                        new List<Position> { positions["Do_stats"] }
                    )
                },
                { "Load B", new Transition(
                    "Load B",
                    new ExponentialDelayGenerator(lambdaBigTruckLoad),
                    new List<Position> {  positions["BF_Q1"] },
                    new List<Position> { positions["B_L"],positions["E_F"] }) },
                { "Load S", new Transition(
                    "Load S",
                    new ExponentialDelayGenerator(lambdaSmallTruckLoad),
                    new List<Position> { positions["SF_Q1"] },
                    new List<Position> { positions["S_L"], positions["E_F"]  }) },

                { "Advance BF_Q2", new Transition(
                    "Advance BF_Q2",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q2"], positions["E_F"], },
                    new List<Position> { positions["BF_Q1"], positions["Q2_F"] }) },
                { "Advance BF_Q3", new Transition(
                    "Advance BF_Q3",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q3"], positions["Q2_F"] },
                    new List<Position> { positions["BF_Q2"], positions["Q3_F"] }) },
                { "Advance BF_Q4", new Transition(
                    "Advance BF_Q4",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q4"], positions["Q3_F"] },
                    new List<Position> { positions["BF_Q3"], positions["Q4_F"] }) },
                { "Advance BF_Q5", new Transition(
                    "Advance BF_Q5",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q5"], positions["Q4_F"] },
                    new List<Position> { positions["BF_Q4"], positions["Q5_F"] }) },
                { "Advance BF_Q6", new Transition(
                    "Advance BF_Q6",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["BF_Q6"], positions["Q5_F"] },
                    new List<Position> { positions["BF_Q5"] }) },

                { "Advance SF_Q2",new Transition(
                    "Advance SF_Q2",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q2"], positions["E_F"], },
                    new List<Position> { positions["SF_Q1"], positions["Q2_F"] }) },
                { "Advance SF_Q3", new Transition(
                    "Advance SF_Q3",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q3"], positions["Q2_F"] },
                    new List<Position> { positions["SF_Q2"], positions["Q3_F"] }) },
                { "Advance SF_Q4", new Transition(
                    "Advance SF_Q4",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q4"], positions["Q3_F"] },
                    new List<Position> { positions["SF_Q3"], positions["Q4_F"] }) },
                { "Advance SF_Q5", new Transition(
                    "Advance SF_Q5",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q5"], positions["Q4_F"] },
                    new List<Position> { positions["SF_Q4"], positions["Q5_F"] }) },
                { "Advance SF_Q6", new Transition(
                    "Advance SF_Q6",
                    new ConstantDelayGenerator(0),
                    new List<Position> { positions["SF_Q6"], positions["Q5_F"] },
                    new List<Position> { positions["SF_Q5"] }) },

                { "Go to grinder B_L", new Transition(
                    "Go to grinder B_L",
                    new ConstantDelayGenerator(3),
                    new List<Position> { positions["B_L"] },
                    new List<Position> { positions["BQ"] }) },
                { "Go to grinder S_L", new Transition(
                    "Go to grinder S_L",
                    new ConstantDelayGenerator(2.5m),
                    new List<Position> { positions["S_L"] },
                    new List<Position> { positions["SQ"] }) },

                { "Go back B", new Transition(
                    "Go back B",
                    new ConstantDelayGenerator(2),
                    new List<Position> {  positions["B unloaded"] },
                    new List<Position> {  positions["BF_Q6"],}) },
                { "Go back S", new Transition(
                    "Go back S",
                    new ConstantDelayGenerator(1.5m),
                    new List<Position> { positions["S unloaded"] },
                    new List<Position> { positions["SF_Q6"] }) },

                { "Unload B", new Transition(
                    "Unload B",
                    new ExponentialDelayGenerator(lambdaBigTruckUnload),
                    new List<Position> { positions["BQ"], positions["G_F"] },
                    new List<Position> { positions["B unloaded"], positions["G_F"] },
                    1) },
                { "Unload S",new Transition(
                    "Unload S",
                    new ExponentialDelayGenerator(lambdaSmallTruckUnload),
                    new List<Position> { positions["SQ"], positions["G_F"] },
                    new List<Position> { positions["S unloaded"], positions["G_F"] }) }
            };

            var statsModule = new ModifiedTransportSystemXlsxStatsModule(positions,interval);

            return new PetriModel()
            {
                Positions = positions,
                Transitions = transitions,
                PetriSim = new PetriSim(transitions.Select(kv => kv.Value).ToList(), statsModule),
                StatsModule = statsModule
            };
        }
    }
}
