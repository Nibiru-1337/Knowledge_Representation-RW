﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Klasa reprezentująca formułę logiczną w postaci DNF
	/// </summary>
	public class AlternativeOfConjunctions:LogicClause
	{
		private readonly IList<UniformConjunction> _conjunctions;

		public IReadOnlyList<UniformConjunction> Conjunctions => _conjunctions.ToList().AsReadOnly();


		public AlternativeOfConjunctions()
		{
			_conjunctions = new List<UniformConjunction>();
		}

		public override bool CheckForState(int state)
		{
			return _conjunctions.Any(conjunction => conjunction.CheckForState(state));
		}

		public void AddConjunction(UniformConjunction uniformConjunction)
		{
			_conjunctions.Add(uniformConjunction);
		}
	}
}