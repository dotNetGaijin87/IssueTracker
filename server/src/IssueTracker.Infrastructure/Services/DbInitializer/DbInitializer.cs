using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Infrastructure.Services.DbInitializer
{
    public interface IDbInitializer
    {
        void RunWithDataForDevelopment();
    }
    public  class DbInitializer : IDbInitializer
    {
        IServiceScopeFactory _serviceProvider;
        public DbInitializer(IServiceScopeFactory serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void RunWithDataForDevelopment()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                SeedDb(context);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SeedDb(IAppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Users.AddRange(new List<User>
                {
                    new User { Id = "User_1", Email = "www.user_1@email.com", Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_2", Email = "www.user_2@email.com", Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_3", Email = "www.user_3@email.com", Role = UserRole.admin , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_4", Email = "www.user_4@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_5", Email = "www.user_5@email.com",  Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_6", Email = "www.user_6@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_7", Email = "www.user_7@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_8", Email = "www.user_8@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_9", Email = "www.user_9@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_10", Email = "www.user_10@email.com", Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_11", Email = "www.user_11@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_12", Email = "www.user_12@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_13", Email = "www.user_13@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_14", Email = "www.user_14@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_15", Email = "www.user_15@email.com",  Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_16", Email = "www.user_16@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_17", Email = "www.user_17@email.com",  Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_18", Email = "www.user_18@email.com", Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_19", Email = "www.user_19@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_20", Email = "www.user_20@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_21", Email = "www.user_21@email.com",  Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_22", Email = "www.user_22@email.com", Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_23", Email = "www.user_23@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_24", Email = "www.user_24@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_25", Email = "www.user_25@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_26", Email = "www.user_26@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_27", Email = "www.user_27@email.com", Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "User_28", Email = "www.user_28@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_29", Email = "www.user_29@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_30", Email = "www.user_30@email.com",  Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "User_31", Email = "www.user_31@email.com",  Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "employee", Email = "www.employee@email.com", Role = UserRole.employee , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                    new User { Id = "admin", Email = "www.admin@email.com", Role = UserRole.admin , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = true },
                    new User { Id = "LordWader", Email = "www.LordWader@email.com", Role = UserRole.manager , RegisteredOn = DateTime.Now, LastLoggedOn = DateTime.Now, IsActivated = false },
                });
            context.Projects.AddRange(new List<Project>
                {
                    new Project
                    {
                        Id = "Project_1",
                        Summary  = "Project A description",
                        Description = @"# This is a very important [React](https://facebook.github.io/react/) project

> Current status

1. Ready to be unit tested.
2. Integration tests are on the way.
3. There are minor bugs remaining.

```javascript
import React from 'react';
import ReactDOM from 'react-dom';
import MEDitor from '@uiw/react-md-editor';

```

",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_1",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Id = "Issue_1",
                                Type = IssueType.Bug,
                                Summary =  "Issue_1 summary",
                                Description = "Issue_1 description",
                                CreationTime = DateTime.Now,
                                CompletionTime = DateTime.Now,
                                Progress = IssueProgress.Closed,
                                Priority = IssuePriority.Low,
                                CreatedBy = "User_1",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "employee", IsPinnedToKanban = true},
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_2", IsPinnedToKanban = true},
                                },
                                Comments = new List<Comment>
                                {
                                    new Comment { Id = "Comment_1", Content = "This should be resolved by the end of the month.", CreationTime = DateTime.Now, UserId = "User_1" },
                                    new Comment { Id = "Comment_2", Content = "Which customer is this issue concerned with?", CreationTime = DateTime.Now.AddHours(-4), UserId = "User_3" },
                                    new Comment { Id = "Comment_3", Content = "I will close it.", CreationTime = DateTime.Now.AddDays(-8), UserId = "User_2"  },
                                    new Comment { Content = "I will check it.", CreationTime = DateTime.Now.AddDays(-3), UserId = "User_3"  },
                                    new Comment { Content = "The documents are ready.", CreationTime = DateTime.Now.AddMonths(-2), UserId = "LordWader"  },
                                    new Comment { Content = "66666666666666666666", CreationTime = DateTime.Now, UserId = "User_1"  },
                                    new Comment { Content = "7777777777777777777777", CreationTime = DateTime.Now.AddHours(-4), UserId = "User_3"  },
                                    new Comment { Content = "888888888888888888888888", CreationTime = DateTime.Now.AddDays(-8), UserId = "User_2"  },
                                    new Comment { Content = "999999999999999999999999999", CreationTime = DateTime.Now.AddDays(-3), UserId = "User_3"  },
                                    new Comment { Content = "10_10_10_10_10_10_10_10_", CreationTime = DateTime.Now.AddMonths(-2), UserId = "LordWader"  },
                                    new Comment { Content = "11_11_11_11_11_11_11_11_", CreationTime = DateTime.Now.AddMonths(-2), UserId = "LordWader"  },
                                    new Comment { Content = "12_12_12_12_12_12_12_12_", CreationTime = DateTime.Now.AddMonths(-2), UserId = "LordWader"  },
                                },
                            },
                            new Issue
                            {
                                Id = "Issue_2",
                                Type = IssueType.Improvement,
                                Summary =  "IssueA2 description",
                                CreationTime = DateTime.Now,
                                CompletionTime = DateTime.Now,
                                Progress = IssueProgress.InProgress,
                                Priority = IssuePriority.Medium,
                                CreatedBy = "User_2",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_5", IsPinnedToKanban = true},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "employee", IsPinnedToKanban = true},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "User_22", IsPinnedToKanban = true},
                                },
                                 Comments = new List<Comment>
                                {
                                    new Comment { Id = "Comment_4", Content = "Two days later the issue was settled.", CreationTime = DateTime.Now, UserId = "User_2" },
                                    new Comment { Id = "Comment_5", Content = "I was able to take care of that issue, though.", CreationTime = DateTime.Now.AddHours(-4), UserId = "User_4" },
                                    new Comment { Id = "Comment_6", Content = "It would only confuse the issue further.", CreationTime = DateTime.Now.AddDays(-8), UserId = "User_5"  },
                                },
                            },
                            new Issue
                            {
                                Id = "Issue_3",
                                Type = IssueType.Improvement,
                                Summary =  "Issue_3 summary",
                                Description = "Issue_3 description",
                                CreationTime = DateTime.Now,
                                Progress = IssueProgress.ToBeTested,
                                Priority = IssuePriority.High,
                                CreatedBy = "User_3",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_5", IsPinnedToKanban = true},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "employee", IsPinnedToKanban = true},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "User_21", IsPinnedToKanban = false, KanbanRowPosition = 1},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_6",
                                Type = IssueType.Improvement,
                                Summary =  "IssueA6 description",
                                CreationTime = DateTime.Now,
                                Progress = IssueProgress.OnHold,
                                Priority = IssuePriority.Critical,
                                CreatedBy = "User_1",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_1"},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "admin"},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_4",
                                Type = IssueType.Improvement,
                                Summary =  "IssueA4 description",
                                CreationTime = DateTime.Now,
                                Progress = IssueProgress.Canceled,
                                Priority = IssuePriority.Critical,
                                CreatedBy = "User_1",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_1"},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "admin"},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_5",
                                Type = IssueType.Improvement,
                                Summary =  "Issue_5 summary",
                                CreationTime = DateTime.Now,
                                Progress = IssueProgress.ToDo,
                                Priority = IssuePriority.Critical,
                                CreatedBy = "User_1",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_1"},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "admin"},
                                }
                            },
                        },
                    },
                    new Project
                    {
                        Id = "Project_2",
                        Summary  = "Project_2 summary",
                        Description  = "Project_2 description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_3",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Id = "Issue_11",
                                Type = IssueType.Bug,
                                Summary = "IssueB1 description",
                                CreationTime = DateTime.Now.AddYears(-2),
                                CompletionTime = DateTime.Now.AddYears(-1),
                                Progress = IssueProgress.Canceled,
                                Priority = IssuePriority.Critical,
                                CreatedBy = "User_3",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_3"},
                                    new Permission { IssuePermission = IssuePermission.None, UserId  = "User_4"},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_12",
                                Type = IssueType.Improvement,
                                Summary = "IssueB2 description",
                                CreationTime = DateTime.Now,
                                Progress = IssueProgress.ToDo,
                                Priority = IssuePriority.Medium,
                                CreatedBy = "User_3",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "admin"},
                                    new Permission { IssuePermission = IssuePermission.CanModify,      UserId  = "User_4"},
                                }
                            },
                        },
                    },
                    new Project
                    {
                        Id = "Project_3",
                        Summary  = "Project C description",
                        CreationTime = DateTime.Now,
                        CompletionTime = DateTime.Now,
                        Progress = ProjectProgress.Open,
                        CreatedBy = "User_5",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Id = "Issue_21",
                                Type = IssueType.Improvement,
                                Summary = "IssueC1 description",
                                CreationTime = DateTime.Now.AddYears(-5),
                                CompletionTime = DateTime.Now.AddYears(-1),
                                Progress = IssueProgress.Canceled,
                                Priority = IssuePriority.Critical,
                                CreatedBy = "User_2",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "admin"},
                                    new Permission { IssuePermission = IssuePermission.None, UserId  = "User_6"},
                                }
                            },
                        },
                    },
                    new Project
                    {
                        Id = "Project_4",
                        Summary  = "Project D description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_1",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Id = "Issue_31",
                                Type = IssueType.Bug,
                                Summary = "IssueD1 description",
                                CreationTime = DateTime.Now,
                                CompletionTime = DateTime.Now,
                                Progress = IssueProgress.OnHold,
                                Priority = IssuePriority.Low,
                                CreatedBy = "User_3",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "admin"},
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_2"},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_32",
                                Type = IssueType.Bug,
                                Summary = "IssueD2 description",
                                CreationTime = DateTime.Now,
                                CompletionTime = DateTime.Now,
                                Progress = IssueProgress.OnHold,
                                Priority = IssuePriority.Medium,
                                CreatedBy = "User_1",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_3"},
                                    new Permission { IssuePermission = IssuePermission.CanModify,      UserId  = "admin"},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_33",
                                Type = IssueType.Improvement,
                                Summary = "IssueD3 description",
                                CreationTime = DateTime.Now,
                                Progress = IssueProgress.ToBeTested,
                                Priority = IssuePriority.Critical,
                                CreatedBy = "User_6",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "admin"},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "User_2"},
                                }
                            },
                        },
                    },
                    new Project
                    {
                        Id = "Project_5",
                        Summary  = "Project E description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_1",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Id = "Issue_41",
                                Type = IssueType.Bug,
                                Summary = "IssueE1 description",
                                CreationTime = DateTime.Now,
                                CompletionTime = DateTime.Now,
                                Progress = IssueProgress.OnHold,
                                Priority = IssuePriority.Low,
                                CreatedBy = "User_1",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "User_1"},
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_2"},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_42",
                                Type = IssueType.Bug,
                                Summary = "IssueE2 description",
                                CreationTime = DateTime.Now,
                                CompletionTime = DateTime.Now,
                                Progress = IssueProgress.OnHold,
                                Priority = IssuePriority.Medium,
                                CreatedBy = "User_3",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_3", IsPinnedToKanban = true, KanbanRowPosition = 2},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "User_4", IsPinnedToKanban = false, KanbanRowPosition = 1},
                                }
                            },
                            new Issue
                            {
                                Id = "Issue_43",
                                Type = IssueType.Improvement,
                                Summary = "IssueE3 description",
                                CreationTime = DateTime.Now,
                                Progress = IssueProgress.ToBeTested,
                                Priority = IssuePriority.Critical,
                                CreatedBy = "User_5",
                                Permissions = new List<Permission>
                                {
                                    new Permission { IssuePermission = IssuePermission.CanDelete, UserId  = "User_1"},
                                    new Permission { IssuePermission = IssuePermission.CanModify, UserId  = "User_2"},
                                }
                            },
                        },
                    },
                    new Project
                    {
                        Id = "Project_6",
                        Summary  = "Project F description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_5",
                    },
                    new Project
                    {
                        Id = "Project_7",
                        Summary  = "Project G description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_6",
                    },
                    new Project
                    {
                        Id = "Project_8",
                        Summary  = "Project G description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Open,
                        CreatedBy = "User_6",
                    },
                    new Project
                    {
                        Id = "Project_9",
                        Summary  = "Project I description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_7",
                    },
                    new Project
                    {
                        Id = "Project_10",
                        Summary  = "Project J description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_6",
                    },
                    new Project
                    {
                        Id = "Project_11",
                        Summary  = "Project K description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Closed,
                        CreatedBy = "User_7",
                    },
                    new Project
                    {
                        Id = "Project_12",
                        Summary  = "Project L description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Open,
                        CreatedBy = "User_6",
                    },
                    new Project
                    {
                        Id = "Project_13",
                        Summary  = "Project_13 summary",
                        Description  = "Project_13 description",
                        CreationTime = DateTime.Now,
                        Progress = ProjectProgress.Open,
                        CreatedBy = "User_7",
                    }

                });
        }
    }
}
