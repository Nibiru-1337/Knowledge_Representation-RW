using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_Frontend.InputsViewModels
{
    public class ActionViewModel
    {
        public string Action { get; set; }

        public ActionViewModel(string action)
        {
            this.Action = action;
        }
    }
}