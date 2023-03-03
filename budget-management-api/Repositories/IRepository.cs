using System.Linq.Expressions;

namespace budget_management_api.Repositories;

public interface IRepository<TEntity>
{
        Task<TEntity> Save(TEntity entity);
        TEntity Attach(TEntity entity);
        Task<TEntity?> FindById(Guid id);
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> criteria);
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> criteria, string[] includes);
        Task<IEnumerable<TEntity>> FindAll();
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> criteria);
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
}