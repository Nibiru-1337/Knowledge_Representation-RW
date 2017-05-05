
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RW-tests")]
namespace RW_backend.Models.GraphModels
{
    /// <summary>
    /// Reprezentuje stan świata - wartości fluentów
    /// </summary>
    public class State
    {
		public int FluentValues { get; }

		public State(int fluentValues)
        {
            FluentValues = fluentValues;
        }

        

        public bool FluentValue(int fluentNumber) => (FluentValues & (1 << fluentNumber)) > 0;

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