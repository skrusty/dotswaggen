using System.Collections.Generic;
using dotswaggen.CSharpModel.DataTypes;

namespace dotswaggen.Interfaces
{
    public interface IDataType
    {
        string Description { get; set; }
        string Name { get; set; }
        string ParentType { get; set; }
        IEnumerable<DataProperty> Properties { get; set; }
    }
}