using System.Collections.Generic;
using dotswaggen.CSharpModel.Operations;

namespace dotswaggen.Interfaces
{
    public interface IApi
    {
        string Description { get; set; }
        string Path { get; set; }
        IEnumerable<Operation> Operations { get; set; }
    }
}