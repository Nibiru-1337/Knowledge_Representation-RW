using System;
using RW_backend.Models;

namespace RW_backend.Logic
{
    /// <summary>
    /// Logika odpowiedzialna za modelowanie świata i udzielanie odpowiedzi na kwerendy
    /// </summary>
    public class BackendLogic
    {
		WorldConstructor WorldConstructor = new WorldConstructor();


        //TODO budowanie reprezentaji świata i odpowiadanie na kwerendy
        public World CalculateWorld(Model model)
        {
	        return WorldConstructor.CalculateWorld(model);
        }


    }
}
