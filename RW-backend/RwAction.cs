using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("RW-tests")]
namespace RW_backend
{
    public class RwAction
    {
        private Dictionary<string, bool> _conditions;
        private Dictionary<string, bool> _results;
        public RwAction(Dictionary<string, bool> conditions, Dictionary<string, bool> results )
        {
            _conditions = conditions;
            _results = results;
        }
    }
}
