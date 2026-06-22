using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Interfaces;

public interface IUserCredentialsService
{
    UserCredentials Get();
}