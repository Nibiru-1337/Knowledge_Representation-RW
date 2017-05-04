namespace RW_backend.Models.Clauses.LogicClauses
{
    /// <summary>
    /// Formuła logiczna fluentów w postaci CNF.
    /// </summary>
    public abstract class LogicClause
    {
        public int PositiveFluents { get; private set; }
        public int NegatedFluents { get; private set; }

        protected LogicClause()
        {
            PositiveFluents = 0;
            NegatedFluents = 0;
        }

        public abstract bool CheckForState(int state);

        public void AddFluent(int fluentId, bool negated)
        {
            if (negated)
            {
                NegatedFluents = NegatedFluents | (1 << fluentId);
            }
            else
            {
                PositiveFluents = PositiveFluents | (1 << fluentId);
            }
        }

        public void DeleteFluent(int fluentId, bool negated)
        {
            if (negated)
            {
                NegatedFluents = NegatedFluents & (~(1 << fluentId));
            }
            else
            {
                PositiveFluents = PositiveFluents & (~(1 << fluentId));
            }
        }
    }
}
