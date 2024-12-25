using AutoMapper;
using SharedLibrary.Wrappers;

namespace UserService.Application.Mappers;

public class UserAutoMapperProfile : Profile
{
    public UserAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap(typeof(PagedResponse<>), typeof(PagedResponse<>))
           .ConvertUsing(typeof(PagedResponseConverter<,>));
    }
}
public class PagedResponseConverter<TSource, TDestination> : ITypeConverter<PagedResponse<TSource>, PagedResponse<TDestination>>
{
    public PagedResponse<TDestination> Convert(PagedResponse<TSource> source, PagedResponse<TDestination> destination, ResolutionContext context)
    {
        var mappedData = context.Mapper.Map<TDestination>(source.Data);
        var result = new PagedResponse<TDestination>(mappedData, source.PageNumber, source.PageSize);
        result.TotalRecords = source.TotalRecords;
        result.TotalPages = source.TotalPages;
        return result;
    }
}