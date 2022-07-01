using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Infrastructure.Persistence.Data;

namespace ParsFile.Infrastructure.Repositories.Identity
{
    public class WithdrawalRepository : Repository<Withdrawal>, IWithdrawalRepository
    {
        public WithdrawalRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Withdrawal withdrawal)
        {
            _db.Update(withdrawal);
        }
    }
}
