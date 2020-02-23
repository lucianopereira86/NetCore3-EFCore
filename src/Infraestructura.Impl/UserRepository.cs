using Entidades;

namespace Infraestructura.Impl
{
    public class UserRepository : BaseRepository<Usuario>, IUserRepository
    {
        public UserRepository(CablemodemContext context) : base(context)
        {
        }
    }
}
