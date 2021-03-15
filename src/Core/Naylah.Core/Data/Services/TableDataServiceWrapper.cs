﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public class TableDataServiceWrapper<TEntity, TModel, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        protected readonly TableDataService<TEntity, TModel, TIdentifier> tableDataService;

        public TableDataServiceWrapper(TableDataService<TEntity, TModel, TIdentifier> tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        public IRepository<TEntity, TIdentifier> Repository { get { return tableDataService.Repository; } }

        //public Func<IQueryable<TEntity>, IQueryable> Projection { get { return tableDataService.Projection; } }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> Ordering { get { return tableDataService.Ordering; } }

        public virtual TModel ToModel(TEntity entity)
        {
            return tableDataService.ToModel<TModel>(entity);
        }

        public virtual void UpdateEntity(TEntity e, TModel model, UpsertType upsertType)
        {
            tableDataService.UpdateEntity(e, model, upsertType);
        }

        [Obsolete("Use CreateEntity instead of this")]
        public virtual TEntity ToEntity(TModel model, UpsertType upsertType)
        {
            return tableDataService.CreateEntity(model, upsertType);
        }

        public virtual TEntity CreateEntity(TModel model, UpsertType upsertType)
        {
            return tableDataService.CreateEntity(model, upsertType);
        }

        public virtual void GenerateId(TEntity entity)
        {
            tableDataService.GenerateId(entity);
        }

        public virtual IQueryable<TEntity> GetEntities()
        {
            return tableDataService.GetEntities();
        }

        public virtual IQueryable<TModel> Project(IQueryable<TEntity> entities)
        {
            return tableDataService.Project<TModel>(entities);
        }

        public virtual Task<bool> CommitAsync() => tableDataService.CommitAsync();
    }

    public static class TableDataServiceWrapperExtensions
    {
        public static TableDataServiceWrapper<TEntity, TModel, TIdentifier> CreateWrapper<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> tableDataService)
             where TEntity : class, IEntity<TIdentifier>, IModifiable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            return new TableDataServiceWrapper<TEntity, TModel, TIdentifier>(tableDataService);
        }
    }
}
