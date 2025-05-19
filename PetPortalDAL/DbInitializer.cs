using System;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;
using PetPortalCore.Models;

namespace PetPortalDAL;

public class DbInitializer
{
    private static readonly Random Rand = new();

    public static void Seed(PetPortalDbContext context)
    {
        // === Roles ===
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

        // === Users ===
        if (!context.Users.Any())
        {
            var adminRoleId = context.Roles.First(r => r.Name == "Admin").Id;
            var userRoleId = context.Roles.First(r => r.Name == "User").Id;

            var users = new List<UserEntity>();
            for (int i = 1; i <= 10; i++)
            {
                users.Add(new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = $"User{i}",
                    Email = $"user{i}@example.com",
                    PasswordHash = $"hashedPassword{i}",
                    RoleId = i == 1 ? adminRoleId : userRoleId
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // === Projects ===
        if (!context.Projects.Any())
        {
            var users = context.Users.ToList();
            var projectNames = new[]
            {
                "AI Assistant", "E-commerce Platform", "Mobile App", "Game Engine",
                "Data Analytics Dashboard", "Blockchain Explorer", "IoT Monitoring System",
                "Cybersecurity Tool", "Cloud Infrastructure", "Chatbot"
            };

            var projectDescriptions = new[]
            {
                "Smart AI assistant for daily tasks.",
                "Full-stack e-commerce platform with payment gateway.",
                "Cross-platform mobile app for social networking.",
                "Custom game engine in C++.",
                "Interactive dashboard for business analytics.",
                "Tool for blockchain transaction analysis.",
                "Real-time IoT device monitoring system.",
                "Security software to detect vulnerabilities.",
                "Cloud-based deployment infrastructure.",
                "Conversational chatbot for customer support."
            };

            var requirementsList = new[]
            {
                "ML engineers and data scientists",
                "Frontend and backend developers",
                "Mobile developers (iOS/Android)",
                "C++ developers with graphics experience",
                "Data analysts and visualization experts",
                "Blockchain specialists",
                "Embedded systems engineers",
                "Security researchers",
                "DevOps engineers",
                "NLP engineers and UX designers"
            };

            var teamDescriptions = new[]
            {
                "Team of ML engineers and product managers.",
                "Web developers and QA engineers.",
                "Mobile developers and UI/UX designers.",
                "Engineers with game physics experience.",
                "Data scientists and BI analysts.",
                "Crypto developers and auditors.",
                "IoT firmware and cloud developers.",
                "Penetration testers and cryptographers.",
                "SREs and infrastructure engineers.",
                "NLP engineers and chat designers."
            };

            var plans = new[] { "Basic", "Pro", "Premium" };
            var results = new[]
            {
                "Deployed AI assistant with high accuracy.",
                "Live e-commerce site with 1M+ users.",
                "Published iOS and Android apps.",
                "Released open-source game engine.",
                "Dashboard used by enterprise clients.",
                "Public blockchain explorer launched.",
                "Scalable IoT monitoring platform.",
                "Vulnerability scanner tool released.",
                "CI/CD pipelines automated.",
                "Enterprise-grade chatbot deployed."
            };

            var projects = new List<ProjectEntity>();
            for (int i = 0; i < 30; i++)
            {
                var ownerId = users[Rand.Next(users.Count)].Id;
                projects.Add(new ProjectEntity
                {
                    Id = Guid.NewGuid(),
                    Name = (projectNames[i % 10] + " (" + i + ") "),
                    Description = projectDescriptions[i % 10],
                    Requirements = requirementsList[i % 10],
                    TeamDescription = teamDescriptions[i % 10],
                    Plan = plans[Rand.Next(plans.Length)],
                    Result = results[i % 10],
                    OwnerId = ownerId,
                    Deadline = DateTime.UtcNow.AddDays(Rand.Next(30, 365)),
                    ApplyingDeadline = DateTime.UtcNow.AddDays(Rand.Next(7, 30)),
                    StateOfProject = Rand.NextDouble() > 0.5 ? StateOfProject.Open : StateOfProject.Closed,
                    Budget = (uint)Rand.Next(500_000, 2_000_000)
                });
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();
        }

        // === UserProjects ===
        if (!context.UserProjects.Any())
        {
            var users = context.Users.ToList();
            var projects = context.Projects.ToList();

            var userProjects = new List<UserProject>();
            foreach (var project in projects)
            {
                // Каждый проект может иметь нескольких участников, кроме владельца
                int participantsCount = Rand.Next(1, 4); // 1–3 участника
                for (int i = 0; i < participantsCount; i++)
                {
                    var randomUser = users[Rand.Next(users.Count)];
                    if (randomUser.Id == project.OwnerId) continue;

                    userProjects.Add(new UserProject
                    {
                        Id = Guid.NewGuid(),
                        UserId = randomUser.Id,
                        ProjectId = project.Id
                    });
                }
            }

            context.UserProjects.AddRange(userProjects);
            context.SaveChanges();
        }

        // === Tags ===
        if (!context.Tags.Any())
        {
            var tagNames = new[]
            {
                "ML", "Ruby", "C#", "Algorithms", "DataScience", "Python", "JavaScript",
                "DevOps", "AI", "Cloud", "Networking", "Security", "Testing", "UI/UX",
                "Mobile", "Frontend", "Backend", "GameDev", "Blockchain", "IoT"
            };

            var tags = new List<TagEntity>();
            foreach (var name in tagNames)
            {
                tags.Add(new TagEntity
                {
                    Id = Guid.NewGuid(),
                    Name = name
                });
            }

            context.Tags.AddRange(tags);
            context.SaveChanges();
        }

        // === ProjectTags ===
        if (!context.ProjectTags.Any())
        {
            var projects = context.Projects.ToList();
            var tags = context.Tags.ToList();

            var projectTags = new List<ProjectTag>();
            foreach (var project in projects)
            {
                int tagCount = Rand.Next(2, 4); // 2–3 тега на проект
                for (int i = 0; i < tagCount; i++)
                {
                    var tag = tags[Rand.Next(tags.Count)];
                    if (projectTags.Exists(pt => pt.ProjectId == project.Id && pt.TagId == tag.Id)) continue;

                    projectTags.Add(new ProjectTag
                    {
                        ProjectId = project.Id,
                        TagId = tag.Id
                    });
                }
            }

            context.ProjectTags.AddRange(projectTags);
            context.SaveChanges();
        }
    }
}