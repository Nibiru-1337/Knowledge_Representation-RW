using System.Collections.Generic;
using RW_backend.Models.GraphModels;

namespace RW_backend.Models
{
    /// <summary>
    /// Reprezentuje modelowany świat za pomocą grafu stanów
    /// </summary>
    public class World
    {
        //TODO implementacja grafu stanów świata, stanów początkowych itd
	    Dictionary<int, LinkedState> Graph;

	    public World(Dictionary<int, LinkedState> graph)
	    {
		    Graph = graph;
	    }

    }
}
