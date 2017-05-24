using System;
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
	        World world = new World(model.FluentsCount, model.AlwaysStatements, model.InitiallyStatements, GetNoninertialFluents(model),
				model.CausesStatements, model.ReleasesStatements, model.AfterStatements, model.ActionsCount);
            return world;
        }


	    private BitSet GetNoninertialFluents(Model model)
	    {
		    BitSetOperator bop = new BitSetOperator();
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
