using System.Collections.Generic;
using DotLiquid;

namespace dotswaggen.CSharpModel.DataTypes
{
    public class DataType : Drop
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string ParentType { get; set; }
        public IEnumerable<DataProperty> Properties { get; set; }
    }
}