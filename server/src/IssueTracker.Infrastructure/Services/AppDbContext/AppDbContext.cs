using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Intrefaces;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IssueTracker.Infrastructure.Services.AppDbContext;

public class AppDbContext : DbContext, IAppDbContext
{
    private IDateTimeSnapshot _dateTimeSnapshot  { get; }
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IDateTimeSnapshot dateTimeSnapshot)
        : base(options)
    {
        _dateTimeSnapshot  = dateTimeSnapshot; 
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }
    DatabaseFacade IAppDbContext.Database { get => base.Database; }

    public async Task<int> SaveChangesAsync()
    {
        var dateTimeSnapshot = _dateTimeSnapshot.Now;

        foreach (var entry in ChangeTracker.Entries())
        {

            switch (entry.Entity)
            {
                case Project project:
                    UpdateProjectTimestamps(dateTimeSnapshot, entry, project);    
                    break;

                case Issue issue: 
                    UpdateIssueTimestamps(dateTimeSnapshot, entry, issue);       
                    break;

                case Permission permission:
                    UpdatePermissionTimestamps(dateTimeSnapshot, entry, permission);
                    break;

                case Comment post:
                    UpdateCommentTimestamps(dateTimeSnapshot, entry, post);
                    break;

                case User user:
                    UpdateUserTimestamps(dateTimeSnapshot, entry, user);
                    break;

                default:
                    break;
            }
        }
        return await base.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    private void UpdateProjectTimestamps(DateTime dateTimeSnapshot, EntityEntry entry, Project project)
    {
        if (entry.State == EntityState.Added)
        {
            project.CreationTime = dateTimeSnapshot;
        }
        if (project.Progress == ProjectProgress.Closed)
        {
            project.CompletionTime = dateTimeSnapshot;
        }
        else
        {
            project.CompletionTime = null;
        }
    }

    private void UpdateIssueTimestamps(DateTime dateTimeSnapshot, EntityEntry entry, Issue issue)
    {
        if (entry.State == EntityState.Added)
        {
            issue.CreationTime = dateTimeSnapshot;
        }
        if (issue.Progress == IssueProgress.Closed || issue.Progress == IssueProgress.Canceled)
        {
            issue.CompletionTime = dateTimeSnapshot;
        }
        else
        {
            issue.CompletionTime = null;
        }
    }

    private void UpdatePermissionTimestamps(DateTime dateTimeSnapshot, EntityEntry entry, Permission permission)
    {
        if (entry.State == EntityState.Added)
        {
            permission.CreationTime = dateTimeSnapshot;
        }
    }

    private void UpdateCommentTimestamps(DateTime dateTimeSnapshot, EntityEntry entry, Comment post)
    {
        if (entry.State == EntityState.Added)
        {
            post.CreationTime = dateTimeSnapshot;
        }
    }

    private void UpdateUserTimestamps(DateTime dateTimeSnapshot, EntityEntry entry, User user)
    {
        if (entry.State == EntityState.Added)
        {
            user.RegisteredOn = dateTimeSnapshot;
            user.LastLoggedOn = dateTimeSnapshot;
        }
        else if (entry.State == EntityState.Modified)
        {
            user.LastLoggedOn = dateTimeSnapshot;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(e => e.Id)
                .HasName("PK_User");

            entity.Property(e => e.Id)
                .HasMaxLength(30);

            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(e => e.Role)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.Role)
                .HasConversion(v =>
                    v.ToString(),
                    v => EnumUtils.StringToUserRole(v));
        });



        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects");

            entity.HasKey(e => e.Id)
                .HasName("PK_Project");

            entity.Property(e => e.Id)
                .HasMaxLength(50);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50);

            entity.Property(e => e.OwnedBy)
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Summary)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            entity.Property(e => e.Progress)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.Progress)
                .HasConversion(v =>
                    v.ToString(),
                    v => (ProjectProgress)Enum.Parse(typeof(ProjectProgress), v, true));
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.ToTable("Issues");

            entity.HasKey(e => e.Id)
                .HasName("PK_Issue");

            entity.HasOne(p => p.Project)
                .WithMany(b => b.Issues)
                .HasConstraintName("FKC_Project_Issues")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Property(e => e.Id)
                .HasMaxLength(100);

            entity.Property(e => e.Summary)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.HasMany(c => c.Comments)
                .WithOne(e => e.Issue)
                .HasConstraintName("FKC_Issue_Comments")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Type)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.Progress)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.Priority)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.Type)
                .HasConversion(v =>
                    v.ToString(),
                    v => (IssueType)Enum.Parse(typeof(IssueType), v));

            entity.Property(e => e.Progress)
                .HasConversion(v =>
                    v.ToString(),
                    v => (IssueProgress)Enum.Parse(typeof(IssueProgress), v, true));

            entity.Property(e => e.Priority)
                .HasConversion(v =>
                    v.ToString(),
                    v => (IssuePriority)Enum.Parse(typeof(IssuePriority), v, true));


        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");

            entity.HasKey(x => new { x.UserId, x.IssueId });

            entity.Property(x => x.IssuePermission)
                    .IsRequired();

            entity.Property(x => x.CreationTime)
                    .IsRequired();

            entity.Property(e => e.GrantedBy)
                    .HasMaxLength(50);

            entity.HasOne(x => x.User)
                .WithMany(x => x.Issues)
                .HasConstraintName("FKC_User_Issue")
                .HasForeignKey(x => x.UserId);

            entity.HasOne(x => x.Issue)
                .WithMany(x => x.Permissions)
                .HasConstraintName("FKC_Issue_User")
                .HasForeignKey(x => x.IssueId);

            entity.Property(x => x.IssuePermission)
                .HasColumnType("varchar(20)");

            entity.Property(x => x.IssuePermission)
                .HasConversion(x =>
                    x.ToString(),
                    x => (IssuePermission)Enum.Parse(typeof(IssuePermission), x, true));
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");

            entity.HasIndex(e => e.Id)
                .IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .ValueGeneratedOnAdd();

            entity.HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasConstraintName("FKC_User_Comments")
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            entity.HasOne(x => x.Issue)
               .WithMany(x => x.Comments)
               .HasConstraintName("FKC_Issue_Comments")
               .HasForeignKey(x => x.IssueId)
               .IsRequired();

            entity.Property(e => e.Content)
                .HasMaxLength(300)
                .IsUnicode()
                .IsRequired();
        });
    }
}

