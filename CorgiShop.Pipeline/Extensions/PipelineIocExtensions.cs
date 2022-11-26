using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CorgiShop.Pipeline.Extensions;

public static class PipelineIocExtensions
{
    public static void AddMediatrForCrudPipeline(this IServiceCollection services)
    {
        services.AddMediatR(typeof(PipelineIocExtensions).Assembly);
    }

    public static void AddCrudPipeline<TEntity, TDto, TRepositoryInterface, TRepositoryImplementation>(this IServiceCollection services)
        where TRepositoryInterface : class, IRepository<TEntity>
        where TRepositoryImplementation : class, TRepositoryInterface
        where TEntity : class, IRepositoryEntity
        where TDto : class, IDtoEntity
    {
        services.AddTransient<IRepository<TEntity>, TRepositoryImplementation>();
        services.AddTransient<TRepositoryInterface, TRepositoryImplementation>();

        //Commands
        services.AddTransient<IRequestHandler<DeleteCommand<TDto>, Unit>, DeleteCommandHandler<TDto, TEntity>>();

        //Queries
        services.AddTransient<IRequestHandler<GetListPaginatedQuery<TDto>, PaginatedResultsDto<TDto>>, GetListPaginatedQueryHandler<TDto, TEntity>>();
    }

    public static void DecorateCrudPipeline<TEntity, TRepositoryInterface, TRepositoryDecorator>(this IServiceCollection services)
        where TRepositoryInterface : class, IRepository<TEntity>
        where TRepositoryDecorator : class, TRepositoryInterface
        where TEntity : class, IRepositoryEntity
    {
        services.Decorate<IRepository<TEntity>, TRepositoryDecorator>();
        services.Decorate<TRepositoryInterface, TRepositoryDecorator>();
    }
}
