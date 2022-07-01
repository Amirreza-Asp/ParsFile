using ParsFile.Application.Contrcats.Persistense.Repositories;
using ParsFile.Domain.Entities.Content;

namespace ParsFile.Application.Contracts.Repositories.Content
{
    public interface ICategoryRepsitory : IRepository<Category>
    {
        public void Update(Category category);
    }
}
