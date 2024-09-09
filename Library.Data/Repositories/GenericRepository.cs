using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Report;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace Library.Data.Repositories;

/// <inheritdoc />
public class GenericRepository<T> : GenericRepository, IGenericRepository<T> where T : class
{
    protected DbSet<T> dbSet;

    public GenericRepository(ApplicationDbContext context) : base(context)
    {
        dbSet = context.Set<T>();
    }

    public virtual async Task Create(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    public virtual async Task CreateRange(IEnumerable<T> entites)
    {
        await dbSet.AddRangeAsync(entites);
    }

    public virtual void Update(T entity)
    {
        if (entity is BaseModel baseModel)
        {
            baseModel.UpdateDate = DateTime.UtcNow;
        }
        dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        dbSet.Remove(entity);
    }

    public virtual void DeleteAllWhere(Expression<Func<T, bool>> where)
    {
        var entitiesToRemove = dbSet.Where(where);
        dbSet.RemoveRange(entitiesToRemove);
    }

    public virtual IQueryable<T> GetAllAsQueryable(bool trackChanges, int languageId = 1)
    {
        return trackChanges ?
            dbSet.AsQueryable() :
            dbSet.AsNoTracking().AsQueryable();
    }

    public virtual async Task<IEnumerable<T>> GetAll(bool trackChanges, int languageId = 1)
    {
        return trackChanges ?
            await dbSet.ToListAsync() :
            await dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> where, bool trackChanges)
    {
        return trackChanges ?
            await dbSet.Where(where).ToListAsync() :
            await dbSet.AsNoTracking().Where(where).ToListAsync();
    }

    public virtual async Task<T?> GetById(Guid id, bool trackChanges, int languageId = 1) // we cannot implement trackChanges here because of FindAsync, but will override this when needed
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<T?> GetOneWhere(Expression<Func<T, bool>> where, bool trackChanges)
    {
        return trackChanges ?
            await dbSet.FirstOrDefaultAsync(where) :
            await dbSet.AsNoTracking().FirstOrDefaultAsync(where);
    }
}

public class GenericRepository : IGenericRepository
{
    protected ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PopularityReportRow>> GetPopularityReport(string modelName, DateTime start, DateTime end)
    {
        var modelNameParameter = new SqlParameter("@Model", SqlDbType.NVarChar) { Value = modelName };
        var startDateParameter = new SqlParameter("@StartDate", SqlDbType.Date) { Value = start };
        var endDateParameter  = new SqlParameter("@EndDate", SqlDbType.Date) { Value = end };

        var sql = $"EXEC dbo.GetPopularityReport @Model, @StartDate, @EndDate";

        var result = await _context.Set<PopularityReportRow>()
            .FromSqlRaw(sql, modelNameParameter, startDateParameter, endDateParameter)
            .AsNoTracking()
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<AnnualReportRow>> GetAnnualReport(string modelName, int year)
    {
        var modelNameParameter = new SqlParameter("@Model", SqlDbType.NVarChar) { Value = modelName };
        var yearParameter = new SqlParameter("@Year", SqlDbType.Int) { Value = year };

        var sql = $"EXEC dbo.GetAnnualReport @Model, @Year";

        var result = await _context.Set<AnnualReportRow>()
            .FromSqlRaw(sql, modelNameParameter, yearParameter)
            .AsNoTracking()
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<BooksDamagedReportRow>> GetBooksDamagedReport(string modelName, DateTime start, DateTime end)
    {
        var modelNameParameter = new SqlParameter("@Model", SqlDbType.NVarChar) { Value = modelName };
        var startDateParameter = new SqlParameter("@StartDate", SqlDbType.Date) { Value = start };
        var endDateParameter = new SqlParameter("@EndDate", SqlDbType.Date) { Value = end };

        var sql = $"EXEC dbo.GetBooksDamagedReport @Model, @StartDate, @EndDate";

        var result = await _context.Set<BooksDamagedReportRow>()
            .FromSqlRaw(sql, modelNameParameter, startDateParameter, endDateParameter)
            .AsNoTracking()
            .ToListAsync();

        return result;
    }
}
