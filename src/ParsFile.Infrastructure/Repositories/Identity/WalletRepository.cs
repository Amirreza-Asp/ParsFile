using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Infrastructure.Persistence.Data;

namespace ParsFile.Infrastructure.Repositories.Identity
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(ApplicationDbContext db) : base(db)
        {
        }

        public bool Exists(string username)
        {
            return _db.Wallet.Any(u => u.UserName == username);
        }

        public void UpdateCash(String username, double cash)
        {
            var wallet = _db.Wallet.FirstOrDefault(x => x.UserName == username);

            if (wallet == null)
                return;

            wallet.Cash += (decimal)cash;
            _db.Update(wallet);
        }
    }
}
