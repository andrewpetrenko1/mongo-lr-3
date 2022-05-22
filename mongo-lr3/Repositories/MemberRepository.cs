using mongo_lr3.Models;
using mongo_lr3.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace mongo_lr3.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository (IMongoDatabase database) : base (database) 
        {
            
        }
    }
}
