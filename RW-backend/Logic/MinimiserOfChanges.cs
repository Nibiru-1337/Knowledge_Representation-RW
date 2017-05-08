using System.Collections.Generic;
using System.Linq;
using RW_backend.Models.BitSets;

namespace RW_backend.Logic
{
	public class MinimiserOfChanges
	{


		public List<State> MinimaliseChanges(State initialState, List<State> reachableStates, int releasedFluents = 0, int noninertialFluents = 0)
		{

			List<KeyValuePair<BitSet, List<State>>> changesSets =
				new List<KeyValuePair<BitSet, List<State>>>(reachableStates.Count);

			foreach (State reachableState in reachableStates)
			{
				int changes = (initialState.SetOfDifferentValuesThan(reachableState.FluentValues)
								| releasedFluents) & ~noninertialFluents;

				Logger.Log("changes = " + changes);
				if (changesSets.Count == 0)
				{
					changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes), new List<State>() {reachableState}));
					continue;
				}
				
				for (int i = 0; i < changesSets.Count; i++)
				{
					if (changesSets[i].Key.Set == changes)
					{
						changesSets[i].Value.Add(reachableState);
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
						}
						else if (changesSets[i].Key.IsSupersetOf(changes))
						{
							// np.: wcześniej mieliśmy New = { {a, b}, {a, c} }
							// ale teraz trafia nam się {a}
							// więc oczywiście trzeba usunąć
							DeleteAllWorseThan(changes, changesSets);
							changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes),
								new List<State>() { reachableState }));
							break;
						}
						else
						{
							changesSets.Add(new KeyValuePair<BitSet, List<State>>(new BitSet(changes),
								new List<State>() {reachableState}));
						}

					}
				}
					
			}

			return changesSets.Aggregate(new List<State>(), (list, p) =>
			{
				list.AddRange(p.Value);
				return list;
			});
		}

		private void DeleteAllWorseThan(int changes, List<KeyValuePair<BitSet, List<State>>> changesSet)
		{
			changesSet.RemoveAll(p => p.Key.Set != changes && p.Key.IsSupersetOf(changes));
		}



	}
}
