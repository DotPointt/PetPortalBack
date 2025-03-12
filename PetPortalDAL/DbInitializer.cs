using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL;

public class DbInitializer
{
    public static void Seed(PetPortalDbContext context)
    {
        if (!context.Roles.Any())
        {
            var roles = new List<RoleEntity>
            {
                new RoleEntity { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Admin" },
                new RoleEntity { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "User" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        // Добавление пользователей
        if (!context.Users.Any())
        {
            var adminRoleId = context.Roles.First(r => r.Name == "Admin").Id;
            var userRoleId = context.Roles.First(r => r.Name == "User").Id;

            var users = new List<UserEntity>
            {
                new UserEntity { Id = Guid.Parse("a1b2c3d4-e5f6-4712-8a9b-c0d123456789"), Name = "Alice", Email = "alice@example.com", PasswordHash = "hashedPassword1", RoleId = adminRoleId },
                new UserEntity { Id = Guid.Parse("b1c2d3e4-f5a6-4823-9b0c-d1e234567890"), Name = "Bob", Email = "bob@example.com", PasswordHash = "hashedPassword2", RoleId = userRoleId },
                new UserEntity { Id = Guid.Parse("c1d2e3f4-a5b6-4934-0c1d-e2f345678901"), Name = "Charlie", Email = "charlie@example.com", PasswordHash = "hashedPassword3", RoleId = userRoleId },
                new UserEntity { Id = Guid.Parse("d1e2f3a4-b5c6-4a45-1d2e-f3a456789012"), Name = "Diana", Email = "diana@example.com", PasswordHash = "hashedPassword4", RoleId = userRoleId },
                new UserEntity { Id = Guid.Parse("e1f2a3b4-c5d6-4b56-2e3f-a4b567890123"), Name = "Eve", Email = "eve@example.com", PasswordHash = "hashedPassword5", RoleId = userRoleId }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // Добавление проектов
        if (!context.Projects.Any())
        {
            var users = context.Users.ToList();

            var projects = new List<ProjectEntity>
            {
                new ProjectEntity
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    Name = "Project 1",
                    Description = "Description 1",
                    OwnerId = users.First(user => user.Name == "Alice").Id
                },
                new ProjectEntity
                {
                    Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                    Name = "Project 2",
                    Description = "Description 2",
                    OwnerId = users.First(user => user.Name == "Alice").Id
                },
                new ProjectEntity
                {
                    Id = Guid.Parse("33333333-4444-5555-6666-777777777777"),
                    Name = "Project 3",
                    Description = "Description 3",
                    OwnerId = users.First(user => user.Name == "Bob").Id
                },
                new ProjectEntity
                {
                    Id = Guid.Parse("44444444-5555-6666-7777-888888888888"),
                    Name = "Project 4",
                    Description = "Description 4",
                    OwnerId = users.First(user => user.Name == "Bob").Id
                },
                new ProjectEntity
                {
                    Id = Guid.Parse("55555555-6666-7777-8888-999999999999"),
                    Name = "Project 5",
                    Description = "Description 5",
                    OwnerId = users.First(user => user.Name == "Charlie").Id
                }
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();
        }

        // Добавление связей пользователей и проектов
        if (!context.UserProjects.Any())
        {
            var users = context.Users.ToList();
            var projects = context.Projects.ToList();

            var userProjects = new List<UserProject>
            {
                new UserProject { Id = Guid.NewGuid(), UserId = users.First(u => u.Name == "Alice").Id, ProjectId = projects.First(p => p.Name == "Project 1").Id },
                new UserProject { Id = Guid.NewGuid(), UserId = users.First(u => u.Name == "Alice").Id, ProjectId = projects.First(p => p.Name == "Project 2").Id },
                new UserProject { Id = Guid.NewGuid(), UserId = users.First(u => u.Name == "Bob").Id, ProjectId = projects.First(p => p.Name == "Project 3").Id },
                new UserProject { Id = Guid.NewGuid(), UserId = users.First(u => u.Name == "Bob").Id, ProjectId = projects.First(p => p.Name == "Project 4").Id },
                new UserProject { Id = Guid.NewGuid(), UserId = users.First(u => u.Name == "Charlie").Id, ProjectId = projects.First(p => p.Name == "Project 5").Id }
            };

            context.UserProjects.AddRange(userProjects);
            context.SaveChanges();
        }

            // Добавление тегов
        if (!context.Tags.Any())
        {
            var tags = new List<TagEntity>
            {
                new TagEntity { Id = Guid.NewGuid(), Name = "ML" },
                new TagEntity { Id = Guid.NewGuid(), Name = "Ruby" },
                new TagEntity { Id = Guid.NewGuid(), Name = "C#" },
                new TagEntity { Id = Guid.NewGuid(), Name = "Algorithms" },
                new TagEntity { Id = Guid.NewGuid(), Name = "DataScience" },
                new TagEntity { Id = Guid.NewGuid(), Name = "Python" },
                new TagEntity { Id = Guid.NewGuid(), Name = "JavaScript" },
                new TagEntity { Id = Guid.NewGuid(), Name = "DevOps" },
                new TagEntity { Id = Guid.NewGuid(), Name = "AI" },
                new TagEntity { Id = Guid.NewGuid(), Name = "Cloud" }
            };

                context.Tags.AddRange(tags);
                context.SaveChanges();
        }

            // Добавление связей проектов и тегов
            if (!context.ProjectTags.Any())
        {
            var projects = context.Projects.ToList();
            var tags = context.Tags.ToList();

            var projectTags = new List<ProjectTag>
            {
                // Связываем Project 1 (по Id) с тегами ML и DataScience
                new ProjectTag { ProjectId = projects[0].Id, TagId = tags.First(t => t.Name == "ML").Id },
                new ProjectTag { ProjectId = projects[0].Id, TagId = tags.First(t => t.Name == "DataScience").Id },

                // Связываем Project 2 (по Id) с тегами Ruby и DevOps
                new ProjectTag { ProjectId = projects[1].Id, TagId = tags.First(t => t.Name == "Ruby").Id },
                new ProjectTag { ProjectId = projects[1].Id, TagId = tags.First(t => t.Name == "DevOps").Id },

                // Связываем Project 3 (по Id) с тегами C# и Algorithms
                new ProjectTag { ProjectId = projects[2].Id, TagId = tags.First(t => t.Name == "C#").Id },
                new ProjectTag { ProjectId = projects[2].Id, TagId = tags.First(t => t.Name == "Algorithms").Id },

                // Связываем Project 4 (по Id) с тегами Python и AI
                new ProjectTag { ProjectId = projects[3].Id, TagId = tags.First(t => t.Name == "Python").Id },
                new ProjectTag { ProjectId = projects[3].Id, TagId = tags.First(t => t.Name == "AI").Id },

                // Связываем Project 5 (по Id) с тегами JavaScript и Cloud
                new ProjectTag { ProjectId = projects[4].Id, TagId = tags.First(t => t.Name == "JavaScript").Id },
                new ProjectTag { ProjectId = projects[4].Id, TagId = tags.First(t => t.Name == "Cloud").Id }
            };

            context.ProjectTags.AddRange(projectTags);
            context.SaveChanges();
        }
    }
}