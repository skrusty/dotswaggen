using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotswaggen.CSharpModel.DataTypes;

namespace dotswaggen.CSharpModel.Operations
{
    public class Operation : DotLiquid.Drop
    {
        public IEnumerable<Parameter> Parameters { get; set; }
        public string Description { get; set; }
        public string ReturnType { get; set; }
        public string Nickname { get; set; }
        public HttpMethod Method { get; set; }
        public IEnumerable<Response> Responses { get; set; }
    }
}
