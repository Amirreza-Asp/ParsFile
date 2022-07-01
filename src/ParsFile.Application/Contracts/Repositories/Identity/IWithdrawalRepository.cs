using ParsFile.Application.Contrcats.Persistense.Repositories;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Application.Contracts.Repositories.Identity
{
    public interface IWithdrawalRepository : IRepository<Withdrawal>
    {
        public void Update(Withdrawal withdrawal);
    }
}
