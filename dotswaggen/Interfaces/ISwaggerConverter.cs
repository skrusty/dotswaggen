using DotLiquid;

namespace dotswaggen.Interfaces
{
    public interface ISwaggerConverter
    {
        IApi[] Apis { get; }
        IDataType[] Models { get; }
        string DefaultExtension { get; }
        void RegisterSafeTypes();
    }
}