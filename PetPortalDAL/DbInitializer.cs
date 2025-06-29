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
                    StateOfProject = (StateOfProject)Rand.Next(1, 4),
                    IsBusinesProject = Rand.NextDouble() > 0.5,
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
            var tagEntries = new Dictionary<string, string>
            {
                { "2402428c-1d7e-401a-b3b0-9225c4ea3ce2", "Security" },
                { "5b0c48dd-ac98-4c5c-9b14-226b486bfbf8", "Cloud" },
                { "632bf880-3ea3-4ebf-b2df-e6dcdb365a83", "UI/UX" },
                { "6527ccfd-3ea3-4ce6-8b6a-22a8d22784b5", "Algorithms" },
                { "69e23e02-b3ad-40ec-8943-0d669538fa30", "AI" },
                { "82d2c032-f4e8-4e3b-b662-9d6dc7f5ccf8", "Mobile" },
                { "85e7bdf3-2aef-4f0c-aa54-0de1d9b45d58", "Frontend" },
                { "8f1c11f6-8d48-493d-981e-6ba9a1317d5f", "IoT" },
                { "911128ec-76b1-490d-a1ed-48d825fc5f3c", "Python" },
                { "96b04e08-9ef1-4d5f-8c74-0b304d4991ea", "GameDev" },
                { "a82d8cb8-82f2-4642-9267-4398dcac3e18", "DevOps" },
                { "a9b23834-ae48-4eaf-a4fe-eccd526df246", "Backend" },
                { "acd41984-2712-486b-b369-ea0798562f1b", "Testing" },
                { "bce3ecde-ef3e-4937-b01b-1a82fdea473b", "C#" },
                { "cd048c50-997f-4783-94e7-31f864af9379", "ML" },
                { "d55e6dda-5d82-42dc-b8bf-087c42a8be4c", "JavaScript" },
                { "d70b7f68-e7c0-4166-9b4e-312d994da59b", "DataScience" },
                { "e8eb785f-ce8d-40eb-bb3a-5e24e67efde5", "Blockchain" },
                { "ed106e6d-45df-4541-bc13-8ad47473d22e", "Ruby" },
                { "f91ec361-2778-4ce4-bf86-07e4235cca7c", "Networking" }
            };

            var tags = new List<TagEntity>();
            foreach (var entry in tagEntries)
            {
                tags.Add(new TagEntity
                {
                    Id = Guid.Parse(entry.Key),
                    Name = entry.Value
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

            foreach (var project in projects)
            {
                int tagCount = Rand.Next(2, 4); // 2–3 тега на проект
                HashSet<Guid> addedTagIds = new(); // Чтобы избежать дубликатов тегов для одного проекта

                for (int i = 0; i < tagCount; i++)
                {
                    var tag = tags[Rand.Next(tags.Count)];

                    // Проверяем, не добавили ли мы уже этот тег
                    if (project.ProjectTags.Any(pt => pt.TagId == tag.Id) || addedTagIds.Contains(tag.Id))
                        continue;

                    project.ProjectTags.Add(new ProjectTag
                    {
                        ProjectId = project.Id,
                        TagId = tag.Id
                    });

                    addedTagIds.Add(tag.Id);
                }
            }

            context.SaveChanges();
        }
    }
}