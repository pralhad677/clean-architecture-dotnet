namespace Bookify.Application.Abstraction.Email;

public interface IEmailService
{
    Task SendAsync(Domain.Users.Email recipient ,string subject ,string body);
}