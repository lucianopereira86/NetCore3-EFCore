using Entidades;

namespace Infraestructura.Impl
{
    public class CablemodemRepository : BaseRepository<Cablemodem>, ICablemodemRepository
    {
        public CablemodemRepository(CablemodemContext context) : base(context)
        {
        }
    }
}
