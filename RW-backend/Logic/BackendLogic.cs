using System;
using RW_backend.Models;

namespace RW_backend.Logic
{
    /// <summary>
    /// Logika odpowiedzialna za modelowanie świata i udzielanie odpowiedzi na kwerendy
    /// </summary>
    public class BackendLogic
    {
		


        //TODO budowanie reprezentaji świata i odpowiadanie na kwerendy
        public World CalculateWorld(Model model)
        {
			// 1. tworzenie listy wszystkich możliwych stanów (uwzględniając wszystkie always)
			// 2. tworzenie krawędzi na podstawie Causes i Releases
			//		dla każdej akcji znajdujemy wszystkie możliwe causes i releases
			//		dla każdego zdania causes znajdujemy wszystkie możliwe przejścia do stanów
			//		minimalizujemy zbiory na podstawie tego co otrzymaliśmy i zdań releases + fluentów nieinertnych
			//		(UWAGA: podczas rezolucji chyba też będziemy musieli minimalizować? nie wiem...)
			//	3. zapisujemy gdzieś After	


	        throw new NotImplementedException();
        }
    }
}
