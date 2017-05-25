//#define EXTENDED_DEBUG
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using RW_backend.Logic.Queries;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;

namespace RW_backend.Models.World
{
    /// <summary>
    /// Reprezentuje modelowany świat za pomocą grafu stanów
    /// </summary>
    public class World
    {
	    public const int MaxFluentCount = 31; // TODO: czemu nie 32?

        // so far the constructor creates an array of all possible states
        // without any edges in between them (edges are added with AddCauses)
        public  IList<State> States { get; private set; }
		public int FluentsCount { get; private set; }

        //TODO delete if ActionIds not needed in World class
        // information about the actions in this world
        public IList<int> ActionIds { get; private set; }

        //connections b/w states are represented with a chain of dictonaries
        // [int(ActionId)] [State(starting state)] -> List<AgentSetChecker>
        public Dictionary<int, Dictionary<State, IList<AgentSetChecker>>> Connections { get; private set; }
		public List<State> InitialStates { get; private set; } 
		public BitSet NonInertialFluents { get; private set; }
		public Dictionary<int, Dictionary<State, List<ReleasesWithAgentsSet>>> ReleasedFluents { get; private set; }
	    public bool Inconsistent { get; private set; } = false;


        public World(int fluentsCount, IList<LogicClause> alwaysList, IList<LogicClause> initiallyList, 
			BitSet nonInertialFluents, IList<Causes> causesList, IList<Releases> releasesList, IList<After> afterList, int actionsCount)
        {
	        Initialize(fluentsCount, alwaysList, initiallyList, nonInertialFluents);
			AddCauses(causesList, actionsCount);
			AddReleases(releasesList, actionsCount);
			AddAfters(afterList);
        }

	    private void Initialize(int fluentsesCount, IList<LogicClause> alwaysList,
		    IList<LogicClause> initiallyList,
		    BitSet nonInertialFluents)
	    {
			NonInertialFluents = nonInertialFluents;
			FluentsCount = fluentsesCount;
			if (fluentsesCount > MaxFluentCount)
				throw new ArgumentException("max fluent count = " + MaxFluentCount);

			int totalNodes = (int)1 << fluentsesCount;
			Connections = new Dictionary<int, Dictionary<State, IList<AgentSetChecker>>>();
			States = new List<State>(totalNodes);
			ActionIds = new List<int>();

			for (int i = 0; i < totalNodes; i++)
			{
				if (alwaysList != null && alwaysList.Count > 0)
				{
					bool goodToGo = true;
					foreach (var always in alwaysList)
					{
						if (!always.CheckForState(i))
						{
							goodToGo = false;
							break;
						}
					}
					if (goodToGo) States.Add(new State(i));
				}
				else
				{
					States.Add(new State(i));
				}
			}
			if (initiallyList != null)
			{
				InitialStates =
					States.Where(
						state =>
							initiallyList.All(initially => initially.CheckForState(state.FluentValues)))
						.ToList();
			}
			else InitialStates = States.ToList();
		}

        //add the edges concerning a particular causes action
        private void AddCauses(IList<Causes> causesList, int actionCount)
        {
            //for every unique ActionID
            for (int i = 0; i < actionCount; i++)
            {
				Logger.Log("<ACTION> " + i);

                var stateToAgentSetCheckers = new Dictionary<State, IList<AgentSetChecker>>(States.Count);
                //for every starting state
                foreach (var startingState in States)
                {
#if DEBUG
					Logger.Log("state = " + startingState);
#endif

					//TODO check assumption that actionCount is numer of unique ActionIDs, and that they are always sequential
					List<Causes> sameID = causesList.Where(a => a.Action == i).ToList();
                    var ascList = new List<AgentSetChecker>(sameID.Count);
                    //for each action with the same ActionID
                    foreach (var causesClause in sameID)
                    {
#if DEBUG
						Logger.Log("causes nr " + causesClause);
#endif
                        //if there is no conditions or state satisfies conditions
                        if (causesClause.InitialCondition == null 
							|| causesClause.InitialCondition.CheckForState(startingState.FluentValues))
                        {
#if DEBUG
							Logger.Log("state satisfies condition");
#endif
                            //get all states that have proper result fluents as caused by action
                            List<State> possibleResults = new List<State>();

                            foreach (var endingState in States)
                            {
#if DEBUG && EXTENDED_DEBUG
                                Logger.Log("checking for " + endingState + ", logic clause = " + causesClause.Effect);
#endif
                                if (causesClause.Effect.CheckForState(endingState.FluentValues))
                                {
#if DEBUG && EXTENDED_DEBUG
									Logger.Log("(pass)");
#endif
                                    possibleResults.Add(endingState);
                                }
                                else
                                {
#if DEBUG && EXTENDED_DEBUG
	                                Logger.Log("(not)");
#endif
                                }
                            }

							// Minimalisation in building world
							// scenerio:
							// SHOOT by Bob causes !alive if loaded
							// SHOOT by Bob causes !loaded
	                        var result = possibleResults;
                            ascList.Add(new AgentSetChecker(causesClause.AgentsSet.AgentSet, result));
                        }
                    }
					// TODO: jak będzie sprawiać kłopoty, to wywalić
					// ...raczej sprawia kłopoty :(
	                // ascList = CompressAgentsSetCheckers(ascList);
                    stateToAgentSetCheckers.Add(startingState, ascList);
                }
                Connections.Add(i, stateToAgentSetCheckers);
                //TODO delete if ActionIds not needed in World class
                ActionIds.Add(i);
            }
        }

	    private List<AgentSetChecker> CompressAgentsSetCheckers(List<AgentSetChecker> ascList)
	    {
			// O(liczba zdań causes^2 * )
		    List<AgentSetChecker> compressed = new List<AgentSetChecker>(ascList.Count);
			compressed.Add(ascList[0]);
		    for (int indexAsc = 1; indexAsc < ascList.Count; indexAsc++) // teraz chcemy dodać i-ty zbiór
		    {
			    bool shouldBeInserted = true;

				AgentSetChecker ascChecker = ascList[indexAsc];

			    for (int indexCompressed = 0; indexCompressed < compressed.Count; indexCompressed++) // porównujemy z j-tym ze skompresowanego
			    {

				    AgentSetChecker compChecker = compressed[indexCompressed];

				    if (ascChecker.AgentsSet.AgentSet == compChecker.AgentsSet.AgentSet)
				    {
					    // wywalamy z compressed[j] te, których nie ma w ascList[i]
					    compChecker.Edges.RemoveAll(state => !ascChecker.Edges.Contains(state)); // O(liczba stanów)
						// i nie wrzucamy do compressed, bo już mamy przecięcie i jest dobrze
					    shouldBeInserted = false;
					    break; // bo skoro już był tamten, to wszystko załatwił
				    }
					else if (ascChecker.AgentsSet.IsSubsetOf(compChecker.AgentsSet.AgentSet))
					{
						// nowy zbiór jest podzbiorem skompresowanego
						// asc = {a, b}
						// compressed = {a, b, c}
						// czyli stany, do których dojdziemy z compressed[j], muszą też należeć do asc[i]
						compChecker.Edges.RemoveAll(state => !ascChecker.Edges.Contains(state)); // O(liczba stanów)
						// ale wciąż dorzucamy do zbioru
					}
					else if (ascChecker.AgentsSet.IsSupersetOf(compChecker.AgentsSet.AgentSet))
					{
						// teraz stany, do których dojdziemy z asc, muszą należeć do compressed
						ascChecker.Edges.RemoveAll(state => !compChecker.Edges.Contains(state));
					}
			    }

			    if (shouldBeInserted)
			    {
				    compressed.Add(ascChecker);
			    }
		    }

		    return compressed;

	    }

	    private void AddReleases(IList<Releases> releasesList, int actionCount)
	    {
			

			BitSetFactory bitSetFactory = new BitSetFactory();
			ReleasedFluents = new Dictionary<int, Dictionary<State, List<ReleasesWithAgentsSet>>>(ActionIds.Count);

		    if (releasesList == null || releasesList.Count == 0)
			    return;


			//for every unique ActionID
			for (int i = 0; i < actionCount; i++)
			{
				Logger.Log("<ACTION> " + i);

				var stateToReleasesWithAgentsSet = new Dictionary<State, List<ReleasesWithAgentsSet>>();

				//for every starting state
				foreach (var startingState in States)
				{
#if DEBUG
					Logger.Log("state = " + startingState);
#endif

					//TODO check assumption that actionCount is numer of unique ActionIDs, and that they are always sequential
					List<Releases> sameID = releasesList.Where(a => a.Action == i).ToList();

					var ascList = new List<ReleasesWithAgentsSet>(sameID.Count);
					//for each action with the same ActionID
					foreach (var releaseClause in sameID)
					{
#if DEBUG
						Logger.Log("releases nr " + releaseClause);
#endif
						//if there is no conditions or state satisfies conditions
						if (releaseClause.InitialCondition == null
							|| releaseClause.InitialCondition.CheckForState(startingState.FluentValues))
						{
#if DEBUG
							Logger.Log("state satisfies condition");
#endif
							ascList.Add(new ReleasesWithAgentsSet(releaseClause.AgentsSet.AgentSet,
								new BitSet(bitSetFactory.CreateFromOneElement(releaseClause.FluentReleased))));
						}
					}
					stateToReleasesWithAgentsSet.Add(startingState, ascList);
				}
				ReleasedFluents.Add(i, stateToReleasesWithAgentsSet);
			}



		}

	    private void AddAfters(IList<After> afterList)
	    {
			//BitSetFactory factory = new BitSetFactory();
		    if (afterList == null)
			    return;
		    var factory = new LogicClausesFactory();
			foreach (After after in afterList)
		    {
			    if (after.Always)
			    {
					List<State> toDelete = new List<State>(InitialStates.Count);
				    foreach (State initialState in InitialStates)
				    {
					    AfterQuery query = new AfterQuery(after.Program, factory.CreateOnlyOneStateEnabledClause(initialState.FluentValues), 
							true, after.Effect);
					    var result = query.Evaluate(this);
					    if (!result.IsTrue)
					    {
						    toDelete.Add(initialState);
					    }
				    }
				    foreach (State state in toDelete)
				    {
					    InitialStates.Remove(state);
				    }
			    }
			    else
			    {
				    AfterQuery possibly = new AfterQuery(after.Program, null, false, after.Effect);
				    var result = possibly.Evaluate(this);
				    if (!result.IsTrue)
				    {
					    Inconsistent = true; // sprzeczność
					    return; // nie ma po co dalej się męczyć
				    }
			    }
		    }
	    }
    }
}
