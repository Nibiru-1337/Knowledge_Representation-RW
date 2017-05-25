using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models.BitSets;
using RW_backend.Models.World;

namespace RW_tests
{
	class TestUtilities
	{
		const int MaxNumberOfElementsSetInInt = sizeof (int)*8;
		public static string WriteOutBitSet(int set)
		{
			BitSetOperator bop = new BitSetOperator();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < MaxNumberOfElementsSetInInt; i++)
			{
				sb.Append(bop.GetValue(set, i) ? "1" : "0");
			}
			return sb.ToString();
		}

		public static string WriteOutWorld(World world)
		{
			StringBuilder sb = new StringBuilder();
			if (world.Connections.Count == 0)
			{
				sb.Append("world is empty");
			}
			foreach (KeyValuePair<int, Dictionary<State, IList<AgentSetChecker>>> connection in world.Connections)
			{
				sb.Append("for action = " + connection.Key).AppendLine();
				foreach (KeyValuePair<State, IList<AgentSetChecker>> pair in connection.Value)
				{
					sb.Append("\tfor state = ").Append(pair.Key).AppendLine();
					foreach (AgentSetChecker setChecker in pair.Value)
					{
						sb.Append("\t\tfor agents set = ").Append(setChecker.AgentsSet).AppendLine();
						foreach (State state in setChecker.Edges)
						{
							sb.Append("\t\t\tcan go to state ").Append(state).AppendLine();
						}
					}

				}


			}


			return sb.ToString();

		}


		public static string WriteOutWorld(World world, List<string> fluentsNames, List<string> agentsNames, List<string> actionsNames)
		{
			StringBuilder sb = new StringBuilder();
			if (world.Connections.Count == 0)
			{
				sb.Append("world is empty");
			}
			foreach (KeyValuePair<int, Dictionary<State, IList<AgentSetChecker>>> connection in world.Connections)
			{
				sb.Append("for action = " + actionsNames[connection.Key]).AppendLine();
				foreach (KeyValuePair<State, IList<AgentSetChecker>> pair in connection.Value)
				{
					sb.Append("\tfor state = ").Append(GetFluentsFromSet(pair.Key.FluentValues, fluentsNames)).AppendLine();
					foreach (AgentSetChecker setChecker in pair.Value)
					{
						sb.Append("\t\tfor agents set = ").Append(GetAgentsFromSet(setChecker.AgentsSet.AgentSet, agentsNames)).AppendLine();
						foreach (State state in setChecker.Edges)
						{
							sb.Append("\t\t\tcan go to state ").Append(GetFluentsFromSet(state.FluentValues, fluentsNames)).AppendLine();
						}
					}
				}
			}
			return sb.ToString();
		}

		public static string WriteOutWorldFomInitiallyOnly(World world, List<string> fluentsNames, List<string> agentsNames, List<string> actionsNames)
		{
			StringBuilder sb = new StringBuilder();
			if (world.Connections.Count == 0)
			{
				sb.Append("world is empty");
			}
			foreach (KeyValuePair<int, Dictionary<State, IList<AgentSetChecker>>> connection in world.Connections)
			{
				sb.Append("for action = " + actionsNames[connection.Key]).AppendLine();
				foreach (KeyValuePair<State, IList<AgentSetChecker>> pair in connection.Value)
				{
					if (!world.InitialStates.Contains(pair.Key))
						continue;

					sb.Append("\tfor state = ").Append(GetFluentsFromSet(pair.Key.FluentValues, fluentsNames)).AppendLine();
					foreach (AgentSetChecker setChecker in pair.Value)
					{
						sb.Append("\t\tfor agents set = ").Append(GetAgentsFromSet(setChecker.AgentsSet.AgentSet, agentsNames)).AppendLine();
						foreach (State state in setChecker.Edges)
						{
							sb.Append("\t\t\tcan go to state ").Append(GetFluentsFromSet(state.FluentValues, fluentsNames)).AppendLine();
						}
					}
				}
			}
			return sb.ToString();
		}

		private static string GetAgentsFromSet(int set, List<string> names)
		{
			//BitSetOperator bop = new BitSetOperator();
			BitSet bset = new BitSet(set);
			string allnames = "";
			for (int i = 0; i < names.Count; i++)
			{
				if (bset.ElementValue(i))
				{
					allnames += names[i];
					allnames += ", ";
				}
			}
			return allnames;
		}

		private static string GetFluentsFromSet(int set, List<string> names)
		{
			//BitSetOperator bop = new BitSetOperator();
			BitSet bset = new BitSet(set);
			string allnames = "";
			for (int i = 0; i < names.Count; i++)
			{
				if (!bset.ElementValue(i))
				{
					allnames += "!";
				}
				allnames += names[i];
				allnames += ", ";
			}
			return allnames;
		}
	}
}
