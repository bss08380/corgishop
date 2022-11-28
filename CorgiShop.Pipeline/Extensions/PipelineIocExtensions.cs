using AutoMapper;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base;
using CorgiShop.Pipeline.Base.Handlers;
using CorgiShop.Pipeline.Base.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CorgiShop.Pipeline.Extensions;

public record PipelineConfiguration(bool RegisterAutoMapperTypeConversions);

public static class PipelineIocExtensions
{
    public static void AddMediatrForCrudPipeline(this IServiceCollection services)
    {
        services.AddMediatR(typeof(PipelineIocExtensions).Assembly);
    }

    public static void AddCrudPipeline<TEntity, TDto, TRepositoryInterface, TRepositoryImplementation>(this IServiceCollection services, PipelineConfiguration configuration)
        where TRepositoryInterface : class, IRepository<TEntity>
        where TRepositoryImplementation : class, TRepositoryInterface
        where TEntity : class, IRepositoryEntity
        where TDto : class, IDtoEntity
    {
        services.AddTransient<IRepository<TEntity>, TRepositoryImplementation>();
        services.AddTransient<TRepositoryInterface, TRepositoryImplementation>();

        //Commands
        services.AddTransient<IRequestHandler<DeleteCommand<TDto>, Unit>, DeleteCommandHandler<TDto, TEntity>>();
        services.AddTransient<IRequestHandler<CreateCommand<TDto>, TDto>, CreateCommandHandler<TDto, TEntity>>();
        services.AddTransient<IRequestHandler<UpdateCommand<TDto>, TDto>, UpdateCommandHandler<TDto, TEntity>>();

        //Queries
        services.AddTransient<IRequestHandler<GetListPaginatedQuery<TDto>, PaginatedResultsDto<TDto>>, GetListPaginatedQueryHandler<TDto, TEntity>>();
        services.AddTransient<IRequestHandler<GetByIdQuery<TDto>, TDto>, GetByIdQueryHandler<TDto, TEntity>>();
        services.AddTransient<IRequestHandler<GetCountQuery<TDto>, int>, GetCountQueryHandler<TDto, TEntity>>();

        if (configuration.RegisterAutoMapperTypeConversions)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<TEntity, TDto>();
                config.CreateMap<TDto, TEntity>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }
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
