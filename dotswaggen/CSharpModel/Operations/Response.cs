using DotLiquid;

namespace dotswaggen.CSharpModel.Operations
{
    /// <summary>
    ///     This may need to be expanded to handle 2.0 while maintaining backwards compatibility
    /// </summary>
    public class Response : Drop
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}