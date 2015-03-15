using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotswaggen.CSharpModel.DataTypes;

namespace dotswaggen.CSharpModel.Operations
{
    public class Parameter : DotLiquid.Drop
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public ParameterType Location { get; set; }
    }
}
