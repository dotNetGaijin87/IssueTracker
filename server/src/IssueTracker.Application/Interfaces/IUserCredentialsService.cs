using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Intrefaces;

public interface IUserCredentialsService
{
    UserCredentials Get();
}