using DataFeed.Model;
using System.Threading.Tasks;
using System;

namespace DataFeed.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public interface IECCHRepository : IRepository<MeteringJson>
    {
    }
    public class ECCHRepository : BaseRepository<MeteringJson>, IECCHRepository
    {
        public ECCHRepository(IMongoContext context) : base(context)
        {
        }        
    }
}
