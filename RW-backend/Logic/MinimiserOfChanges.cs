//#define EXTENDED_DEBUG
using System.Collections.Generic;
using System.Linq;
using RW_backend.Models.BitSets;

namespace RW_backend.Logic
{
	public class MinimiserOfChanges
	{


		public List<State> MinimaliseChanges(State initialState, List<State> reachableStates, int releasedFluents = 0, int noninertialFluents = 0)
		{

			return MinimaliseChangesOptimalized(initialState, reachableStates, releasedFluents,
				noninertialFluents);

			List<KeyValuePair<BitSet, List<State>>> changesSets =
				new List<KeyValuePair<BitSet, List<State>>>(reachableStates.Count);

			foreach (State reachableState in reachableStates)
			{
				int changes = (initialState.SetOfDifferentValuesThan(reachableState.FluentValues)
								| releasedFluents) & ~noninertialFluents;
#if DEBUG && EXTENDED_DEBUG
                Logger.Log("changes = " + changes);
#endif
				if (changesSets.Count == 0)
				{
					changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes), new List<State> {reachableState}));
					continue;
				}

				bool inserted = false;
				for (int i = 0; i < changesSets.Count; i++)
				{
					if (changesSets[i].Key.Set == changes)
					{
						changesSets[i].Value.Add(reachableState);
						inserted = true;
					}
					else
					{
						// nie są równe
						// w przypadku, gdy dany zbiór new jest nadzbiorem:
						if (changesSets[i].Key.IsSubsetOf(changes))
						{
							// np. wcześniej zmienialiśmy tylko {a, b}
							// a teraz chcemy {a, b, c}
							// no to bez sensu
							// więc
							// nothing to do
							inserted = true;
						}
						else if (changesSets[i].Key.IsSupersetOf(changes))
						{
							// np.: wcześniej mieliśmy New = { {a, b}, {a, c} }
							// ale teraz trafia nam się {a}
							// więc oczywiście trzeba usunąć
							DeleteAllWorseThan(changes, changesSets);
							changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes),
								new List<State> { reachableState }));
							inserted = true;
							break;
						}
						else
						{
							//changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes),
							//	new List<State>() {reachableState}));
							//break;
						}

					}
				}
				if (!inserted)
				{
					changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes),
						new List<State> {reachableState}));
				}

			}

			return changesSets.Aggregate(new List<State>(), (list, p) =>
			{
				list.AddRange(p.Value);
				return list;
			});
		}

		public List<State> MinimaliseChangesOptimalized(State initialState, List<State> reachableStates, int releasedFluents = 0, int noninertialFluents = 0)
		{

			List<KeyValuePair<BitSet, State>> changesSets =
				new List<KeyValuePair<BitSet, State>>(reachableStates.Count);

			foreach (State reachableState in reachableStates)
			{
				int changes = (initialState.SetOfDifferentValuesThan(reachableState.FluentValues)
								| releasedFluents) & ~noninertialFluents;
#if DEBUG && EXTENDED_DEBUG
                Logger.Log("changes = " + changes);
#endif
				if (changesSets.Count == 0)
				{
					changesSets.Add(new KeyValuePair<BitSet, State>(new BitSet(changes), reachableState ));
					continue;
				}

				bool inserted = false;
				for (int i = 0; i < changesSets.Count; i++)
				{
					if (changesSets[i].Key.Set == changes)
					{
						//changesSets[i].Value.Add(reachableState);
						inserted = true;
					}
					else
					{
						// nie są równe
						// w przypadku, gdy dany zbiór new jest nadzbiorem:
						if (changesSets[i].Key.IsSubsetOf(changes))
						{
							// np. wcześniej zmienialiśmy tylko {a, b}
							// a teraz chcemy {a, b, c}
							// no to bez sensu
							// więc
							// nothing to do
							inserted = true;
						}
						else if (changesSets[i].Key.IsSupersetOf(changes))
						{
							// np.: wcześniej mieliśmy New = { {a, b}, {a, c} }
							// ale teraz trafia nam się {a}
							// więc oczywiście trzeba usunąć
							DeleteAllWorseThan(changes, changesSets);
							changesSets.Add(new KeyValuePair<BitSet, State>(new BitSet(changes),
								 reachableState ));
							inserted = true;
							break;
						}
						else
						{
							//changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes),
							//	new List<State>() {reachableState}));
							//break;
						}

					}
				}
				if (!inserted)
				{
					changesSets.Add(new KeyValuePair<BitSet, State>(new BitSet(changes),
						 reachableState ));
				}
			}

			List<State> response = new List<State>();

			foreach (State reachableState in reachableStates)
			{
				int changes = (initialState.SetOfDifferentValuesThan(reachableState.FluentValues)
								| releasedFluents) & ~noninertialFluents;

				for (int i = 0; i < changesSets.Count; i++)
				{
					if (changesSets[i].Key.Set == changes)
					{
						response.Add(reachableState);
					}
				}
			}

			return response;
		}

		private int GetChanges(State initial, int reachable, int released, int noninertial)
		{
			return (initial.SetOfDifferentValuesThan(reachable) | released) & ~noninertial;
		}

		private void DeleteAllWorseThan(int changes, List<KeyValuePair<BitSet, List<State>>> changesSet)
		{
			changesSet.RemoveAll(p => p.Key.Set != changes && p.Key.IsSupersetOf(changes));
		}

		private void DeleteAllWorseThan(int changes, List<KeyValuePair<BitSet, State>> changesSet)
		{
			changesSet.RemoveAll(p => p.Key.Set != changes && p.Key.IsSupersetOf(changes));
		}


	}
}
