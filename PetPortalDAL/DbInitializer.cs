using System;
using PetPortalCore.Abstractions.Services;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;
using PetPortalCore.Models;

namespace PetPortalDAL;

public class DbInitializer
{
    private static readonly Random Rand = new();
    private static readonly Guid otherRoleId = new Guid("A0000000-0000-0000-0000-000000000000");

    public static async Task Seed(PetPortalDbContext context, IMinioService? minioService = null)
    {
        // === Roles ===
        // Справочник специальностей. Сгруппирован по темам так же, как и на фронтенде
        // (src/data/role-categories.ts) — при добавлении роли туда её нужно добавить и здесь.
        var roleNames = new[]
        {
            // Management
            "Product Manager",
            "Product Owner",
            "Project Manager",
            "Delivery Manager",
            "Engineering Manager",
            "Team Lead",
            "Tech Lead",
            "Scrum Master",
            "Business Analyst",
            "System Architect",

            // Frontend
            "Frontend Developer",
            "React Developer",
            "Vue Developer",
            "Angular Developer",
            "Frontend Architect",

            // Backend
            "Backend Developer",
            "Node.js Developer",
            "Python Developer",
            "Java Developer",
            "C#/.NET Developer",
            "Go Developer",
            "PHP Developer",
            "Ruby Developer",
            "Rust Developer",
            "Database Administrator",

            // Fullstack
            "Fullstack Developer",
            "Web Developer",

            // Mobile
            "Mobile Developer (iOS)",
            "Mobile Developer (Android)",
            "Flutter Developer",
            "React Native Developer",

            // AI / ML / Data
            "AI Researcher",
            "ML Engineer",
            "MLOps Engineer",
            "LLM Engineer",
            "Prompt Engineer",
            "NLP Engineer",
            "Computer Vision Engineer",
            "Data Scientist",
            "Data Engineer",
            "Data Analyst",
            "BI Analyst",

            // DevOps & Infrastructure
            "DevOps Engineer",
            "SRE (Site Reliability Engineer)",
            "Platform Engineer",
            "Kubernetes Engineer",
            "Release Engineer",
            "Cloud Architect",
            "Network Engineer",

            // Hardware & IoT
            "Embedded Systems Engineer",
            "IoT Engineer",
            "Robotics Engineer",
            "Hardware Engineer",

            // Design
            "UI/UX Designer",
            "Product Designer",
            "UX Researcher",
            "Graphic Designer",
            "Motion Designer",
            "Illustrator",
            "3D Artist",

            // QA & Testing
            "QA Engineer",
            "QA Automation Engineer",
            "Manual QA Engineer",
            "Performance Engineer",

            // Security
            "Security Specialist",
            "Security Analyst",
            "DevSecOps Engineer",
            "Penetration Tester",

            // Blockchain & Web3
            "Blockchain Developer",
            "Smart Contract Developer",
            "Web3 Developer",

            // GameDev & XR
            "Game Developer",
            "Game Designer",
            "Level Designer",
            "Unity Developer",
            "Unreal Engine Developer",
            "AR/VR Developer",

            // Content & Marketing
            "Technical Writer",
            "Copywriter",
            "Content Manager",
            "Marketing Specialist",
            "SMM Manager",
            "Community Manager",
            "Localization Specialist"
        };

        // Добавляем только отсутствующие роли, чтобы справочник можно было
        // расширять без пересоздания базы (существующие Id не меняются).
        var existingRoleNames = context.Roles
            .Select(role => role.Name)
            .ToHashSet();

        var newRoles = roleNames
            .Where(roleName => !existingRoleNames.Contains(roleName))
            .Select(roleName => new RoleEntity
            {
                Id = Guid.NewGuid(),
                Name = roleName
            })
            .ToList();

        // Роль "Другое" с легко узнаваемым GUID
        if (!context.Roles.Any(role => role.Id == otherRoleId || role.Name == "Другое"))
        {
            newRoles.Add(new RoleEntity
            {
                Id = otherRoleId,
                Name = "Другое",
                IsSystem = true
            });
        }

        if (newRoles.Count > 0)
        {
            context.Roles.AddRange(newRoles);
            context.SaveChanges();
        }

        // === Users ===
        if (!context.Users.Any())
        {
            var roleIds = context.Roles.Select(r => r.Id).ToList();
            var users = new List<UserEntity>();
            
            
            for (int i = 1; i <= 10; i++)
            {
                var user = (new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = $"User{i}",
                    Email = $"user{i}@example.com",
                    PasswordHash = $"hashedPassword{i}",
                    RoleId = roleIds[Rand.Next(roleIds.Count)],
                    EmailConfirmed = true
                });
                
                // Генерируем аватар и загружаем в MinIO, если сервис доступен
                if (minioService != null)
                {
                    var svgContent = GenerateAvatarSvg();
                    var fileName = $"{user.Id}_avatar.svg";

                    using var stream = new MemoryStream();
                    using var writer = new StreamWriter(stream);
                    await writer.WriteAsync(svgContent);
                    await writer.FlushAsync();
                    stream.Position = 0;

                    try
                    {
                        await minioService.UploadFileAsync(fileName, stream, "image/svg+xml");
                        user.AvatarUrl = fileName;
                    }
                    catch (Exception ex)
                    {
                        // Логирование ошибки (можно заменить на ILogger)
                        Console.WriteLine($"Ошибка загрузки аватара для {user.Name}: {ex.Message}");
                        // Не прерываем инициализацию — продолжаем без аватара
                    }
                }
                users.Add(user);
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

                // последние пять проектов — архивные: срок приёма заявок уже истёк
                var isArchived = i >= 25;

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
                    // чем больше i, тем «свежее» проект — чтобы сортировка по дате была наглядной
                    CreatedDate = DateTime.UtcNow.AddDays(-(30 - i)).AddMinutes(Rand.Next(0, 600)),
                    Deadline = DateTime.UtcNow.AddDays(Rand.Next(30, 365)),
                    ApplyingDeadline = isArchived
                        ? DateTime.UtcNow.AddDays(-Rand.Next(1, 20))
                        : DateTime.UtcNow.AddDays(Rand.Next(7, 30)),
                    StateOfProject = isArchived ? StateOfProject.Archived : StateOfProject.Open,
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
    
    
    
    public static string GenerateAvatarSvg()
    {
        // Пастельный фон
        string bgColor = GeneratePastelColor();

        // Контрастный, но мягкий цвет для пикселей (немного темнее фона или нейтральный)
        // Вариант: использовать белый с низкой прозрачностью ИЛИ слегка затемнённый оттенок
        // Здесь — белый с 30% непрозрачности для хорошей видимости
        
        // string pixelColor = "rgba(255, 255, 255, 0.3)";

        string pixelColor = "rgba(0, 0, 0, 0.2)";
        
        // Генерация симметричного пиксельного узора
        string pattern = GeneratePixelPattern(pixelColor);

        return $@"<svg xmlns='http://www.w3.org/2000/svg' width='200' height='200'>
  <rect width='200' height='200' fill='{bgColor}' />
  {pattern}
</svg>";
    }
    
    private static string GeneratePastelColor()
    {
        // Генерируем мягкие пастельные тона: высокая яркость, умеренная насыщенность
        var r = Random.Shared.Next(180, 256);
        var g = Random.Shared.Next(180, 256);
        var b = Random.Shared.Next(180, 256);
        return $"#{r:X2}{g:X2}{b:X2}";
    }
    
    private static string GeneratePixelPattern(string fillColor)
    {
        const int gridSize = 5;
        const int cellSize = 40; // 200 / 5

        var mask = new bool[gridSize, gridSize];

        // Заполняем левую половину + центр случайно
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x <= gridSize / 2; x++)
            {
                bool fill = Random.Shared.Next(2) == 1;
                mask[y, x] = fill;
                mask[y, gridSize - 1 - x] = fill; // зеркальное отражение
            }
        }

        var rects = new List<string>();
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (mask[y, x])
                {
                    rects.Add($"<rect x='{x * cellSize}' y='{y * cellSize}' width='{cellSize}' height='{cellSize}' fill='{fillColor}' />");
                }
            }
        }

        return string.Join("\n  ", rects);
    }

    /// <summary>
    /// Приведение уже существующих данных к актуальной схеме.
    /// В отличие от <see cref="Seed"/> выполняется в любом окружении: это миграция
    /// данных, а не тестовые записи.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public static void NormalizeExistingData(PetPortalDbContext context)
    {
        // === Нормализация статусов ===
        // Раньше состояний было четыре, сейчас актуальны только «идёт набор» и «в архиве».
        // Старые NotSelected/InProgress приводим к «идёт набор».
        var legacyStateProjects = context.Projects
            .Where(project => project.StateOfProject == StateOfProject.NotSelected
                              || project.StateOfProject == StateOfProject.InProgress)
            .ToList();

        foreach (var project in legacyStateProjects)
        {
            project.StateOfProject = StateOfProject.Open;
        }

        if (legacyStateProjects.Count > 0)
        {
            context.SaveChanges();
        }

        // === Бэкфилл дат публикации ===
        // Раньше CreatedDate нигде не проставлялся, поэтому у старых проектов он пустой,
        // а без него не работает ни сортировка каталога, ни «сначала новые» в моих проектах.
        var projectsWithoutCreatedDate = context.Projects
            .Where(project => project.CreatedDate == null)
            .OrderBy(project => project.Id)
            .ToList();

        if (projectsWithoutCreatedDate.Count > 0)
        {
            var baseDate = DateTime.UtcNow.AddDays(-projectsWithoutCreatedDate.Count);
            for (var i = 0; i < projectsWithoutCreatedDate.Count; i++)
            {
                projectsWithoutCreatedDate[i].CreatedDate = baseDate.AddDays(i);
            }

            context.SaveChanges();
        }
    }
}