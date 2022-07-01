using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Infrastructure.Persistence.Data;

namespace ParsFile.Infrastructure.Repositories.Identity
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
