using PetPortalDAL.Entities;

namespace PetPortalDAL;

public class DbInitializer
{
    public static void Seed(PetPortalDbContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<UserEntity>
            {
                new UserEntity { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@example.com", PasswordHash = "hashedPassword1" },
                new UserEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", PasswordHash = "hashedPassword2" },
                new UserEntity { Id = Guid.NewGuid(), Name = "Charlie", Email = "charlie@example.com", PasswordHash = "hashedPassword3" },
                new UserEntity { Id = Guid.NewGuid(), Name = "Diana", Email = "diana@example.com", PasswordHash = "hashedPassword4" },
                new UserEntity { Id = Guid.NewGuid(), Name = "Eve", Email = "eve@example.com", PasswordHash = "hashedPassword5" }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // Проверка на существование проектов перед их добавлением
        if (!context.Projects.Any())
        {
            var users = context.Users.ToList();

            var projects = new List<ProjectEntity>
            {
                new ProjectEntity
                    { Id = Guid.NewGuid(), Name = "Project 1", Description = "Description 1", OwnerId = users[0].Id },
                new ProjectEntity
                    { Id = Guid.NewGuid(), Name = "Project 2", Description = "Description 2", OwnerId = users[0].Id },
                new ProjectEntity
                    { Id = Guid.NewGuid(), Name = "Project 3", Description = "Description 3", OwnerId = users[1].Id },
                new ProjectEntity
                    { Id = Guid.NewGuid(), Name = "Project 4", Description = "Description 4", OwnerId = users[1].Id },
                new ProjectEntity
                    { Id = Guid.NewGuid(), Name = "Project 5", Description = "Description 5", OwnerId = users[2].Id }
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();
        }
    }
}