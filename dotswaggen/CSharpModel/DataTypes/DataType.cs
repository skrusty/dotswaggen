using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotswaggen.CSharpModel.DataTypes
{
    public class DataType : DotLiquid.Drop
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string ParentType { get; set; }
        public IEnumerable<DataProperty> Properties { get; set; }
    }
}
