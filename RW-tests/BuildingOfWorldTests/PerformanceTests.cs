using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.dotMemoryUnit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Factories;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;

namespace RW_tests.BuildingOfWorldTests
{
	[TestClass]
	public class PerformanceTests
	{
		[DotMemoryUnit(CollectAllocations = true)]
		[TestMethod]
		public void YaleScenerioBobShootExecutableAlways10FluentsTest()
		{
			var memoryCheckPointBefore = dotMemory.Check();

			var world = YaleWorldWithMoreFluents(10);
			var query = YaleExecutableQuery();
			

			//var memoryAfter =
			//	dotMemory.Check(memory => memory.GetDifference(memoryCheckPointBefore).GetSurvivedObjects().GetObjects(where => where.));

			//var memoryAfter =
			//	memoryCheckPointBefore.GetTrafficFrom(memoryCheckPointBefore).AllocatedMemory.SizeInBytes;

			var memoryCheck = dotMemory.Check(memory =>
			{
				Traffic traffic = memory.GetTrafficFrom(memoryCheckPointBefore);
				Console.WriteLine("all");
				Console.WriteLine(GetMemoryString(traffic.AllocatedMemory.SizeInBytes));
				Console.WriteLine("State");
				Console.WriteLine(traffic.Where(obj => obj.Type.Is<State>()).AllocatedMemory.ObjectsCount);
				Console.WriteLine(GetMemoryString(traffic.Where(obj => obj.Type.Is<State>()).AllocatedMemory.SizeInBytes));
				Console.WriteLine("KeyValuePair<int, List<State>>");
				Console.WriteLine(traffic.Where(obj => obj.Type.Is<KeyValuePair<BitSet, List<State>>>()).AllocatedMemory.ObjectsCount);
				Console.WriteLine(GetMemoryString(traffic.Where(obj => obj.Type.Is<KeyValuePair<BitSet, List<State>>>())
					.AllocatedMemory.SizeInBytes));
				Console.WriteLine("List<KeyValuePair<BitSet, State>>");
				Console.WriteLine(traffic.Where(obj => obj.Type.Is<List<KeyValuePair<BitSet, State>>>()).AllocatedMemory.ObjectsCount);
				Console.WriteLine(GetMemoryString(traffic.Where(obj => obj.Type.Is<List<KeyValuePair<BitSet, State>>>())
					.AllocatedMemory.SizeInBytes));
				Console.WriteLine("KeyValuePair<int, List<State>>");
				Console.WriteLine(traffic.Where(obj => obj.Type.Is<KeyValuePair<BitSet, List<State>>>()).AllocatedMemory.ObjectsCount);
				Console.WriteLine(GetMemoryString(traffic.Where(obj => obj.Type.Is<KeyValuePair<BitSet, List<State>>>())
					.AllocatedMemory.SizeInBytes));
			});
			Console.WriteLine("----");


			var result = query.Evaluate(world);

			var memoryAfter = dotMemory.Check(memory =>
			{
				Traffic traffic = memory.GetTrafficFrom(memoryCheckPointBefore);
				Console.WriteLine("all");
				Console.WriteLine(GetMemoryString(traffic.AllocatedMemory.SizeInBytes));
				Console.WriteLine("State");
				Console.WriteLine(traffic.Where(obj => obj.Type.Is<State>()).AllocatedMemory.ObjectsCount);
				Console.WriteLine(GetMemoryString(traffic.Where(obj => obj.Type.Is<State>()).AllocatedMemory.SizeInBytes));
				Console.WriteLine("KeyValuePair<int, List<State>>");
				Console.WriteLine(traffic.Where(obj => obj.Type.Is<KeyValuePair<BitSet, List<State>>>()).AllocatedMemory.ObjectsCount);
				Console.WriteLine(GetMemoryString(traffic.Where(obj => obj.Type.Is<KeyValuePair<BitSet, List<State>>>())
					.AllocatedMemory.SizeInBytes));
				Console.WriteLine("List<KeyValuePair<BitSet, State>>");
				Console.WriteLine(traffic.Where(obj => obj.Type.Is<List<KeyValuePair<BitSet, State>>>()).AllocatedMemory.ObjectsCount);
				Console.WriteLine(GetMemoryString(traffic.Where(obj => obj.Type.Is<List<KeyValuePair<BitSet, State>>>())
					.AllocatedMemory.SizeInBytes));

				Console.WriteLine("snapshot");
				SnapshotDifference diff = memory.GetDifference(memoryCheck);
				//traffic.Where(obj => obj.)
				Console.WriteLine(GetMemoryString(diff.GetNewObjects().SizeInBytes));
				Console.WriteLine(GetMemoryString(diff.GetSurvivedObjects().SizeInBytes));
				Console.WriteLine(GetMemoryString(diff.GetDeadObjects().SizeInBytes));

				Console.WriteLine(diff.ToString());
			});

			Assert.AreEqual(true, result.IsTrue, "Bob should be able to shoot");

		}

		private string GetMemoryString(long sizeInBytes)
		{
			const int kB = 1024;
			const int MB = 1024*kB;
			const int GB = 1024*MB;
			long hereGB = sizeInBytes/GB;
			long hereMB = (sizeInBytes - hereGB*GB)/MB;
			long herekB = (sizeInBytes - hereGB*GB - hereMB*MB)/kB;
			return sizeInBytes + " so " + hereGB + "GB + " + hereMB + " MB + " + herekB + " kb";
		}

		[TestMethod]
		public void YaleScenerioIncreasingFluentsCountTest()
		{
			const int MinimumProblemSize = 10;
			const int DesiredProblemSize = 20;
			int fluentsCount;
			for (fluentsCount = 11; fluentsCount <= DesiredProblemSize; fluentsCount++)
			{
				try
				{
					var world = YaleWorldWithMoreFluents(fluentsCount);
					var query = YaleExecutableQuery();
					Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob should be able to shoot");
				}
				catch (OutOfMemoryException) { break; }
				Console.WriteLine("for " + fluentsCount + " is ok");
			}
			if (fluentsCount > DesiredProblemSize)
			{
				Console.WriteLine("Your RAM size rocks! (or the algorithm was improved)");
				return;
			}
			Console.WriteLine("Your RAM failed at {0} fluents", fluentsCount);
			Assert.IsTrue(fluentsCount > MinimumProblemSize, "Buy more RAM");
		}


		private static ExecutableQuery YaleExecutableQuery()
		{
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			ExecutableQuery query = new ExecutableQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet),
			}, logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, false), true);
			return query;
		}

		private World YaleWorldWithMoreFluents(int fluentsCount)
		{
			Model model = new SimpleYaleScenerioWorldGenerator().GenerateModel();
			model.FluentsCount = fluentsCount;
			World world = new BackendLogic().CalculateWorld(model);
			return world;
		}


	}
}
