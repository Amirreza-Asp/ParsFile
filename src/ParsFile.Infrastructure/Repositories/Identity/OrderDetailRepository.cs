using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Infrastructure.Persistence.Data;

namespace ParsFile.Infrastructure.Repositories.Identity
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void AddMany(IEnumerable<OrderDetail> orderDetails)
        {
            _db.OrderDetail.AddRange(orderDetails);
        }

        public bool Exists(string fileId)
        {
            return _db.OrderDetail.Any(u => u.FileId == fileId);
        }
    }
}
