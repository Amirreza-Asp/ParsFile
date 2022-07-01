using ParsFile.Application.Contrcats.Persistense.Repositories;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Application.Contracts.Repositories.Identity
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void AddMany(IEnumerable<OrderDetail> orderDetails);

        bool Exists(String fileId);
    }
}
