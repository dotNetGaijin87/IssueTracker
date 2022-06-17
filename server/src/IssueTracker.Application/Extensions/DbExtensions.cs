using IssueTracker.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.Extensions;
public static class DbExtensions
{
    public static async Task<bool> CheckIfProjectExists(this IAppDbContext dbContext, string id, CancellationToken ct)
    {
        return await dbContext.Projects.AnyAsync(y => y.Id == id, ct);
    }

    public static async Task<bool> CheckIfIssueExists(this IAppDbContext dbContext, string id, CancellationToken ct)
    {
        return await dbContext.Issues.AnyAsync(y => y.Id == id, ct);
    }
}

