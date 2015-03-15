using System.Collections.Generic;
using DotLiquid;

namespace dotswaggen.CSharpModel.Operations
{
    public class Operation : Drop
    {
        public IEnumerable<Parameter> Parameters { get; set; }
        public string Description { get; set; }
        public string ReturnType { get; set; }
        public string Nickname { get; set; }
        public HttpMethod Method { get; set; }
        public IEnumerable<Response> Responses { get; set; }
    }
}