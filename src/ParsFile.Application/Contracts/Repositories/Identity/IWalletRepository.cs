using ParsFile.Application.Contrcats.Persistense.Repositories;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Application.Contracts.Repositories.Identity
{
    public interface IWalletRepository : IRepository<Wallet>
    {

        public void UpdateCash(String username, double cash);

        public bool Exists(String username);

    }
}
