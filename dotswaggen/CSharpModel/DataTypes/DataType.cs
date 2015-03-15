using System.Collections.Generic;
using dotswaggen.Interfaces;
using DotLiquid;

namespace dotswaggen.CSharpModel.DataTypes
{
    public class DataType : Drop, IDataType
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string ParentType { get; set; }
        public IEnumerable<DataProperty> Properties { get; set; }
    }
}