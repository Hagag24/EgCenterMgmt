
namespace EgCenterMgmt.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected EgCenterMgmtContext _context;

        public Repository(EgCenterMgmtContext context)
        {
            _context = context;
        }
        private IQueryable<T> ApplyDynamicSorting(IQueryable<T> query)
        {
            try
            {
                var entityType = _context.Model.FindEntityType(typeof(T));

                if (entityType == null)
                {
                    throw new InvalidOperationException("لم يتم العثور على الكيان في نموذج EF Core.");
                }

                // البحث عن خاصية DateTime أو DateOnly
                var dateProperty = typeof(T)
                    .GetProperties()
                    .FirstOrDefault(p =>
                        p.PropertyType == typeof(DateTime) ||
                        p.PropertyType == typeof(DateTime?) ||
                        p.PropertyType == typeof(DateOnly) ||
                        p.PropertyType == typeof(DateOnly?));

                if (dateProperty != null)
                {
                    // ترتيب حسب التاريخ 
                    return query
                        .OrderByDescending(e => EF.Property<DateTime?>(e, dateProperty.Name) ?? DateTime.MinValue);
                }

                // الحصول على اسم أول عمود في الجدول
                var firstProperty = entityType.GetProperties().FirstOrDefault();

                if (firstProperty == null)
                {
                    throw new InvalidOperationException("لا يمكن العثور على أي عمود في الكيان.");
                }

                // ترتيب البيانات بناءً على أول عمود
                return query.OrderBy(e => EF.Property<object>(e, firstProperty.Name));

            }
            catch (Exception ex)
            {
                // معالجة الخطأ
                Console.WriteLine($"حدث خطأ أثناء تطبيق الترتيب الديناميكي: {ex.Message}");
                throw new InvalidOperationException("فشل تطبيق الترتيب الديناميكي. يرجى التحقق من الحقول في الكيان.", ex);
            }
        }


        public IEnumerable<T> GetAll() => _context.Set<T>().ToList();
        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, int? numday)
        {
            IQueryable<T> query = _context.Set<T>();
           
            query = ApplyDynamicSorting(query);

            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (numday.HasValue)
            {
                query = query.Take(numday ?? 8);
            }
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        public T GetById(int id) => _context.Set<T>().Find(id)!;
        public T GetById(Expression<Func<T, bool>> criteria, string[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.SingleOrDefault(criteria)!;
        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> criteria, string[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.SingleOrDefaultAsync(criteria);
        }
        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public T Find(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();

            return query.SingleOrDefault(criteria)!;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();

            return await query.SingleOrDefaultAsync(criteria);
        }
        public IEnumerable<T> FindAll()
        {
            IQueryable<T> query = _context.Set<T>();
            query = ApplyDynamicSorting(query);
            return query.ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);
            query = ApplyDynamicSorting(query);
            return query.ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);
            query = query.Skip(skip).Take(take);
            query = ApplyDynamicSorting(query);
            return query.ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (orderBy != null)
            {
                query = orderByDirection == OrderBy.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }
            else
            {
                query = ApplyDynamicSorting(query);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);
            query = ApplyDynamicSorting(query);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);
            query = query.Skip(skip).Take(take);
            query = ApplyDynamicSorting(query);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (orderBy != null)
            {
                query = orderByDirection == OrderBy.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }
            else
            {
                query = ApplyDynamicSorting(query);
            }

            return await query.ToListAsync();
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

        public void Attach(T entity) => _context.Set<T>().Attach(entity);

        public void AttachRange(IEnumerable<T> entities) => _context.Set<T>().AttachRange(entities);

        public int Count() => _context.Set<T>().Count();

        public int Count(Expression<Func<T, bool>> criteria) => _context.Set<T>().Count(criteria);

        public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria) => await _context.Set<T>().CountAsync(criteria);

    }
}


/*
   public IEnumerable<T> GetAll() => _context.Set<T>().ToList();
        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, int? numday)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (numday.HasValue)
            {
                query = query.Take(numday ?? 8);
            }
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        public T GetById(int id) => _context.Set<T>().Find(id)!;
        public T GetById(Expression<Func<T, bool>> criteria, string[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.SingleOrDefault(criteria)!;
        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> criteria, string[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.SingleOrDefaultAsync(criteria);
        }
        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public T Find(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();

            return query.SingleOrDefault(criteria)!;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();

            return await query.SingleOrDefaultAsync(criteria);
        }
        public IEnumerable<T> FindAll()=> _context.Set<T>().ToList();
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();

            return query.Where(criteria).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take) => _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip) => await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync();

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }
*/