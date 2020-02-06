namespace dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}