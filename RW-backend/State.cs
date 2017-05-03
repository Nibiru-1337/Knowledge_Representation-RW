namespace RW_backend
{
    /// <summary>
    /// Reprezentuje stan świata - wartości fluentów
    /// </summary>
    internal class State
    {
        public State(int fluentValues)
        {
            FluentValues = fluentValues;
        }

        public int FluentValues { get; }

        public bool FluentValue(int fluentNumber)
        {
            return (FluentValues & (1 << fluentNumber)) > 0;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = obj as State;
            return FluentValues == other?.FluentValues;
        }
        
        public override int GetHashCode()
        {
            return FluentValues.GetHashCode();
        }
    }
}