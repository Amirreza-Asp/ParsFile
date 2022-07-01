using ParsFile.Application.Contrcats.Persistense.Repositories;
using ParsFile.Domain.Entities.Content;

namespace ParsFile.Application.Contracts.Repositories.Content
{
    public interface IBasketRepository : IRepository<Basket>
    {
        public bool Exists(String userName, String fileId);

    }
}
