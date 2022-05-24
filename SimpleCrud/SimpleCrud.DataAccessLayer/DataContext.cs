using Microsoft.EntityFrameworkCore;
using SimpleCrud.DataAccessLayer.Entities;
using System.Reflection;

namespace SimpleCrud.DataAccessLayer
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var deleteEntity = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

            foreach (var entry in deleteEntity)
            {
                var entity = entry.Entity as Customer;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : class
        {
            var set = Set<T>().AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            return trackingChanges ? set.AsTracking() : set.AsNoTracking();
        }

        public ValueTask<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            var set = Set<T>();
            return set.FindAsync(keyValues);

        }

        public void Delete<T>(T entity) where T : class
        {
            var set = Set<T>();
            set.Remove(entity);
        }


        public void Insert<T>(T entity) where T : class
        {
            var set = Set<T>();
            set.Add(entity);
        }

        public Task SaveAsync()
        {
            return SaveChangesAsync();
        }

        public void Edit<T>(T entity) where T : class
        {
            var set = Set<T>();
            set.Update(entity);
        }
    }
}