namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
    /// Dowolna formuła logiczna
    /// (tak naprawdę nie musi dotyczyć fluentów i stanów)
    /// (mogłaby być równie dobra dla formuł agentowych)
    /// i w sumie wciąż: dla dowolnego zbioru reprezentowanego bitowo
    /// TODO: czy nie zrobić z tego interfejsu? bo można.
    /// </summary>
    public abstract class LogicClause
    {
	    public abstract bool CheckForState(int state);
		public abstract bool IsEmpty();
    }
}
