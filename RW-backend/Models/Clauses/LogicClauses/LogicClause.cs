namespace RW_backend
{
    /// <summary>
    /// Formuła logiczna fluentów w postaci CNF.
    /// </summary>
    public abstract class LogicClause
    {
	    public abstract bool CheckForState(int state);
    }
}
