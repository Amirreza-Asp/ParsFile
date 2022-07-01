namespace ParsFile.Application.Contracts.Services
{
    public interface IEmailService
    {
        Task Execute(string email, string subject, string body);
    }
}
