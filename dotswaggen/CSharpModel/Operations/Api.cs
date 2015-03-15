using System.Collections.Generic;
using dotswaggen.Interfaces;
using DotLiquid;

namespace dotswaggen.CSharpModel.Operations
{
    public class Api : Drop, IApi
    {
        public string Description { get; set; }
        public string Path { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
    }
}