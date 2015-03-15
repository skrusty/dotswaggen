using DotLiquid;

namespace dotswaggen.CSharpModel.Operations
{
    public class Parameter : Drop
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public ParameterType Location { get; set; }
    }
}