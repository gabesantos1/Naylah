﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public class RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        private readonly TableDataService<TEntity, TModel, TIdentifier> service;

        public RangedTableDataServiceOperations(TableDataService<TEntity, TModel, TIdentifier> service)
        {
            this.service = service;
        }

        public IEnumerable<TEntity> ToEntities(IEnumerable<TModel> models, UpsertType upsertType)
        {
            var es = new List<TEntity>();
            foreach (var model in models)
            {
                es.Add(service.ToEntity(model, upsertType));
            }

            return es;
        }

        public IEnumerable<TModel> ToModels(IEnumerable<TEntity> entities)
        {
            var ms = new List<TModel>();
            foreach (var entity in entities)
            {
                ms.Add(service.ToModel(entity));
            }
            return ms;
        }

        public async Task<IEnumerable<TModel>> AddRangeAsync(IEnumerable<TModel> models)
        {
            var imp = TableDataServiceExtensions.GetImplementation<ICommandRangeRepository<TEntity, TIdentifier>>(service.CreateWrapper().Repository);
            var es = ToEntities(models, UpsertType.Insert);
            var entities = await imp.AddAsync(es);

            await service.CreateWrapper().CommitAsync();

            return ToModels(entities);
        }

        public async Task<IEnumerable<TModel>> EditRangeAsync(IEnumerable<TModel> models)
        {
            var imp = TableDataServiceExtensions.GetImplementation<ICommandRangeRepository<TEntity, TIdentifier>>(service.CreateWrapper().Repository);
            var es = ToEntities( models, UpsertType.Update);
            var entities = await imp.EditAsync(es);

            await service.CreateWrapper().CommitAsync();

            return ToModels(entities);
        }

        public async Task RemoveRangeAsync(IEnumerable<TModel> models)
        {
            var imp = TableDataServiceExtensions.GetImplementation<ICommandRangeRepository<TEntity, TIdentifier>>(service.CreateWrapper().Repository);
            var es = ToEntities(models, UpsertType.Update);

            await imp.RemoveAsync(es);
            await service.CreateWrapper().CommitAsync();
        }

    }


    public static class RangedTableDataServiceOperationsExtensions
    {
        public static async Task<IEnumerable<TModel>> AddRangeAsync<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> service, IEnumerable<TModel> models)
            where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            var r = new RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>(service);
            return await r.AddRangeAsync(models);
        }

        public static async Task<IEnumerable<TModel>> EditRangeAsync<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> service, IEnumerable<TModel> models)
            where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            var r = new RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>(service);
            return await r.EditRangeAsync(models);
        }

        public static async Task RemoveRangeAsync<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> service, IEnumerable<TModel> models)
            where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            var r = new RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>(service);
            await r.RemoveRangeAsync(models);
        }


    }
}