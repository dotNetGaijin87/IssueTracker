using IssueTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IssueTracker.Application.Interfaces;

public interface IAppDbContext : IDisposable
{
    DbSet<Project> Projects { get; set; }
    DbSet<Issue> Issues { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync();

    int SaveChanges();
}

