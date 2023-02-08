using DataFeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFeed.Persistence
{
    public interface IUserRepository
    {
        Task<IList<User>> GetCategories();
        Task<int> AddManyAsync(IList<User> documents);
    }

    public class UserRepository : IUserRepository
    {
        readonly IMongoClient db;

        public UserRepository(IMongoClient db)
        {
            this.db = db;
        }

        public async Task<int> AddManyAsync(IList<User> documents)
        {
            db.Collection = "user";
            await db.InsertMany(documents);
            return 1;
        }

        public async Task<IList<User>> GetCategories()
        {
            db.Collection = "user";
            return await db.GetAll<User>();
        }
    }
   
    public interface IMeteringRepository
    {
        Task AddManyAsync(IList<MeteringJson> documents);
    }

    public class MeteringRepository : IMeteringRepository
    {
        readonly IMongoClient db;

        public MeteringRepository(IMongoClient db)
        {
            this.db = db;
        }

        public async Task AddManyAsync(IList<MeteringJson> documents)
        {
            db.Collection = "metering";
            await db.InsertMany(documents);
        }
    }
}
