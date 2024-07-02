using AutoMapper;

namespace Together.Shared.Extensions;

public static class AutoMapperExtensions
{
    private static IMapper _mapper = default!;

    public static IMapper CreateAutoMapperInstance(this IMapper mapper)
    {
        _mapper = mapper;
        return _mapper;
    }
    
    public static TDestination MapTo<TDestination>(this object source)
    {
        return _mapper.Map<TDestination>(source);
    }
    
    public static TDestination MapTo<TDestination>(this object source, 
        Action<IMappingOperationOptions<object, TDestination>> options)
    {
        return _mapper.Map(source, options);
    }
    
    public static TDestination MapTo<TSource, TDestination>(this TSource source, 
        Action<IMappingOperationOptions<TSource, TDestination>> options)
    {
        return _mapper.Map(source, options);
    }
}