namespace TAT.StoreLocator.Core.Interface.IMapper
{
    public interface IBaseMapper<TSource, TDestination>
    {
        TDestination MapModel(TSource source);
        IEnumerable<TDestination> MapList(IEnumerable<TSource> source);
    }
}
