using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Domain.Entities.Content;
using ParsFile.Infrastructure.Persistence.Data;

namespace ParsFile.Infrastructure.Repositories.Content
{
    public class CategoryRepostory : Repository<Category>, ICategoryRepsitory
    {
        public CategoryRepostory(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Category category)
        {
            _db.Category.Update(category);
        }
    }
}
