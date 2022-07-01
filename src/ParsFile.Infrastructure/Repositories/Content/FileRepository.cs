using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Infrastructure.Persistence.Data;

namespace ParsFile.Infrastructure.Repositories.Content
{
    public class FileRepository : Repository<Domain.Entities.Content.File>, IFileRepository
    {
        public FileRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Domain.Entities.Content.File file)
        {
            _db.File.Update(file);
        }



    }
}
