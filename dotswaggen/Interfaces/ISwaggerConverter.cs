using DotLiquid;

namespace dotswaggen.Interfaces
{
    public interface ISwaggerConverter
    {
        IApi[] Apis { get; }
        IDataType[] Models { get; }
        void RegisterSafeTypes();
    }
}