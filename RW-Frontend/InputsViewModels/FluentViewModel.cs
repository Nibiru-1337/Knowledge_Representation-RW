using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_Frontend.InputsViewModels
{
    public class FluentViewModel
    {
        public string Fluent { get; set; }

        public FluentViewModel(string fluent)
        {
            this.Fluent = fluent;
        }
    }
}