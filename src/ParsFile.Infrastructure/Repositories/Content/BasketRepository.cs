using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Domain.Entities.Content;
using ParsFile.Infrastructure.Persistence.Data;

namespace ParsFile.Infrastructure.Repositories.Content
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext db) : base(db)
        {
        }

        public bool Exists(string userName, string fileId)
        {
            return _db.Basket.Any(u => u.UserName == userName && u.FileId == fileId);
        }
    }
}
