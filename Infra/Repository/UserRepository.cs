using NetCore3WebAPI.Infra.Interface;
using NetCore3WebAPI.Infra.Models;

namespace NetCore3WebAPI.Infra.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DBContext context) : base(context)
        {
        }
    }
}
