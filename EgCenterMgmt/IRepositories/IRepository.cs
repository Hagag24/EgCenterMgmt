
namespace EgCenterMgmt.IRepository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        T GetById(Expression<Func<T, bool>> criteria, string[] includes);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> criteria, string[] includes);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, int? numday);

        T Find(Expression<Func<T, bool>> criteria);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
    }
}
