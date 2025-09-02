using System;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;
using PetPortalCore.Models;

namespace PetPortalDAL;

public class DbInitializer
{
    private static readonly Random Rand = new();
    private static readonly Guid otherRoleId = new Guid("A0000000-0000-0000-0000-000000000000");

    public static void Seed(PetPortalDbContext context)
    {
        // === Roles ===
        if (!context.Roles.Any())
        {
            var roleNames = new[]
            {
                "Backend Developer",
                "Frontend Developer",
                "Fullstack Developer",
                "DevOps Engineer",
                "UI/UX Designer",
                "Product Manager",
                "Project Manager",
                "QA Engineer",
                "Security Specialist",
                "Data Scientist",
                "Data Analyst",
                "ML Engineer",
                "AI Researcher",
                "Mobile Developer (iOS)",
                "Mobile Developer (Android)",
                "Game Developer",
                "Embedded Systems Engineer",
                "Blockchain Developer",
                "Cloud Architect",
                "SRE (Site Reliability Engineer)",
                "Technical Writer",
                "Scrum Master",
                "Business Analyst",
                "System Architect",
                "Database Administrator",
                "Network Engineer",
                "Penetration Tester",
                "NLP Engineer",
                "Computer Vision Engineer",
                "AR/VR Developer"
            };

            var roles = new List<RoleEntity>();
            foreach (var roleName in roleNames)
            {
                roles.Add(new RoleEntity
                {
                    Id = Guid.NewGuid(),
                    Name = roleName
                });
            }

            // Роль "Другое" с легко узнаваемым GUID
            roles.Add(new RoleEntity
            {
                Id = otherRoleId,
                Name = "Другое"
            });

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        // === Users ===
        if (!context.Users.Any())
        {
            // var adminRoleId = context.Roles.First(r => r.Name == "Admin").Id;
            // var userRoleId = context.Roles.First(r => r.Name == "User").Id;
            var roleIds = context.Roles.Select(r => r.Id).ToList();

            var users = new List<UserEntity>();
            
            
            for (int i = 1; i <= 10; i++)
            {
                users.Add(new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = $"User{i}",
                    Email = $"user{i}@example.com",
                    PasswordHash = $"hashedPassword{i}",
                    RoleId = roleIds[Rand.Next(roleIds.Count)]
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // === Projects ===
        if (!context.Projects.Any())
        {
            var users = context.Users.ToList();
            var allRoleIds = context.Roles.Select(r => r.Id).ToList();
            
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
            
            var customRoleNames = new[]
            {
                "3D Artist", "Sound Designer", "Legal Advisor", "Project Coordinator",
                "Community Manager", "Tech Support", "Copywriter", "Business Analyst",
                "Hardware Engineer", "AI Trainer", "Localization Specialist", "DevRel"
            };


            var projects = new List<ProjectEntity>();
            var projectRoles = new List<ProjectRole>();
            
            for (int i = 0; i < 30; i++)
            {
                var ownerId = users[Rand.Next(users.Count)].Id;
                var projectId = Guid.NewGuid();
                
                projects.Add(new ProjectEntity
                {
                    Id = projectId,
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
                
                var roleCount = Rand.Next(2, 6);
                var selectedRoleIds = allRoleIds
                    .OrderBy(_ => Rand.Next())
                    .Take(roleCount)
                    .ToList();
                
                foreach (var roleId in selectedRoleIds)
                {
                    var projectRole = new ProjectRole
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = projectId,
                        RoleId = roleId
                    };

                    // Если это роль "Другое" — обязательно задаём CustomRoleName
                    if (roleId == otherRoleId)
                    {
                        projectRole.CustomRoleName = customRoleNames[Rand.Next(customRoleNames.Length)];
                    }

                    projectRoles.Add(projectRole);
                }
                
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();
            
            context.ProjectRoles.AddRange(projectRoles);
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