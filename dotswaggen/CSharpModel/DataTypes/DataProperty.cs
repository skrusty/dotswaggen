using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotswaggen.CSharpModel.DataTypes
{
    public class DataProperty : DotLiquid.Drop
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
