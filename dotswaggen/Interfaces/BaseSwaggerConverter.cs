using dotswaggen.Swagger;
using Api = dotswaggen.CSharpModel.Operations.Api;

namespace dotswaggen.Interfaces
{
    public abstract class BaseSwaggerConverter
    {
        protected BaseSwaggerConverter(ApiDeclaration api)
        {
            Root = api;
        }

        protected ApiDeclaration Root { get; set; }
        public abstract Api[] Apis { get; }
        public abstract IDataType[] Models { get; }
        public abstract string DefaultExtension { get; }
        public abstract void RegisterSafeTypes();
    }
}