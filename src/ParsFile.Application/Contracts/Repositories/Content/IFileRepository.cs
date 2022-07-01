using ParsFile.Application.Contrcats.Persistense.Repositories;


namespace ParsFile.Application.Contracts.Repositories.Content
{
    public interface IFileRepository : IRepository<Domain.Entities.Content.File>
    {
        public void Update(Domain.Entities.Content.File file);
    }
}
