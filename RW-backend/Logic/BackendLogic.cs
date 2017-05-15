using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.World;

namespace RW_backend.Logic
{
    /// <summary>
    /// Logika odpowiedzialna za modelowanie świata i udzielanie odpowiedzi na kwerendy
    /// </summary>
    public class BackendLogic
    {
        public World CalculateWorld(Model model)
        {
            // 1. tworzenie listy wszystkich możliwych stanów (uwzględniając wszystkie always)
            // 2. tworzenie krawędzi na podstawie Causes i Releases
            //		dla każdej akcji znajdujemy wszystkie możliwe causes i releases
            //		dla każdego zdania causes znajdujemy wszystkie możliwe przejścia do stanów
            //		minimalizujemy zbiory na podstawie tego co otrzymaliśmy i zdań releases + fluentów nieinertnych
            //		(UWAGA: podczas rezolucji chyba też będziemy musieli minimalizować? nie wiem...)
            //	3. zapisujemy gdzieś After	


			// TODO: prepare releases and initially info for world

			//BitValueOperator bop = new BitValueOperator();
			//Dictionary<KeyValuePair<int, AgentsSet>, BitSet> reldict 
			//	= new Dictionary<KeyValuePair<int, AgentsSet>, BitSet>(model.ReleasesStatements?.Count ?? 0);
	  //      if ((model.ReleasesStatements?.Count ?? 0) > 0)
	  //      {
		 //       foreach (Releases releases in model.ReleasesStatements)
		 //       {
			//        if(reldict.ContainsKey())
		 //       }
	  //      }

	        World world = new World(model.FluentsCount, model.AlwaysStatements, model.InitiallyStatements, GetNoninertialFluents(model));
            world.AddCauses(model.CausesStatements, model.ActionsCount);
			world.AddReleases(model.ReleasesStatements, model.ActionsCount);

            return world;
        }


	    private BitSet GetNoninertialFluents(Model model)
	    {
		    BitValueOperator bop = new BitValueOperator();
		    int set = 0;
			if(model.NoninertialFluents == null)
				return new BitSet(0);
		    foreach (int fluent in model.NoninertialFluents)
		    {
			    bop.SetFluent(set, fluent);
		    }
			return new BitSet(set);
	    }
    }
}
