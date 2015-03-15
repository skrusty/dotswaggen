using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotswaggen.CSharpModel.Operations
{
    public class Api : DotLiquid.Drop
    {
        public string Description { get; set; }
        public string Path { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
    }
}
