using Microsoft.EntityFrameworkCore;
using PetPortalDAL.Configurations;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class PetPortalDbContext : DbContext
{
    /// <summary>
    /// Конструктор контекста.
    /// </summary>
    /// <param name="options">Параметры контекста.</param>
    public PetPortalDbContext(DbContextOptions<PetPortalDbContext> options)
        : base(options)
    {
    }
        
    # region DbSet

    /// <summary>
    /// Проекты в базе данных.
    /// </summary>
    public DbSet<ProjectEntity> Projects { get; set; }
    
    /// <summary>
    /// Отклики.
    /// </summary>
    public DbSet<RespondEntity> Responds { get; set; }

    public DbSet<ResetPasswordTokenEntity> ResetPasswordTokenEntities { get; set; }

    /// <summary>
    /// Пользователи в базе данных.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Роли пользователей в базе данных.
    /// </summary>
    public DbSet<RoleEntity> Roles { get; set; }
    
    /// <summary>
    /// Теги в базе данных.
    /// </summary>
    public DbSet<TagEntity> Tags { get; set; }
    
    /// <summary>
    /// Образования пользователей.
    /// </summary>
    public DbSet<EducationEntity> Educations { get; set; }
    
    /// <summary>
    /// Опыт работы пользователей.
    /// </summary>
    public DbSet<ExperienceEntity> Experiences { get; set; }
    
    /// <summary>
    /// Стэки пользователей.
    /// </summary>
    public DbSet<StackEntity> Stacks { get; set; }
    
    /// <summary>
    /// Сообщения в чатах.
    /// </summary>
    public DbSet<ChatMessageEntity> ChatMessages { get; set; }
    
    /// <summary>
    /// Комната чата.
    /// </summary>
    public DbSet<ChatRoomEntity> ChatRooms { get; set; }
    
    /// <summary>
    /// Связь пользователей и чатов.
    /// </summary>
    public DbSet<ChatRoomUserEntity> ChatRoomUsers { get; set; }
    
    /// <summary>
    /// Связующая таблица между проектами и тегами (фреймворками).
    /// </summary>
    public DbSet<ProjectTag> ProjectTags { get; set; }
    
    /// <summary>
    /// Связующая таблица между проектами и ролями (необходимые в проекте специалисты)
    /// </summary>
    public DbSet<ProjectRole> ProjectRoles { get; set; }
    
    /// <summary>
    /// Связь пользователей и проектов.
    /// </summary>
    public DbSet<UserProject> UserProjects { get; set; }
    
    #endregion

    /// <summary>    
    /// Конфигурация моделей.
    /// </summary>
    /// <param name="builder">Конструктор моделей.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Применение конфигурации моделей
            
        builder.ApplyConfiguration(new ProjectConfigurations());
        builder.ApplyConfiguration(new UserConfigurations());
        builder.ApplyConfiguration(new RoleConfigurations());
        builder.ApplyConfiguration(new TagConfigurations());
        builder.ApplyConfiguration(new ProjectTagConfiguration());
        builder.ApplyConfiguration(new ChatMessageConfigurations());
        builder.ApplyConfiguration(new ChatRoomConfiguration());
        builder.ApplyConfiguration(new ChatRoomUserConfiguration());
        builder.ApplyConfiguration(new ResetPasswordTokensConfigurations());
        builder.ApplyConfiguration(new EducationConfiguration());
        builder.ApplyConfiguration(new ExperienceConfiguration());
        builder.ApplyConfiguration(new StackConfiguration());
        builder.ApplyConfiguration(new RespondConfiguration());
        builder.ApplyConfiguration(new ProjectRoleConfiguration());
        
        #endregion

        #region Настройка связей между моделями

        builder.Entity<ProjectEntity>()
            .HasOne(project => project.Owner)
            .WithMany()
            .HasForeignKey(project => project.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserEntity>()
            .HasOne(user => user.RoleEntity)
            .WithMany(role => role.Users)
            .HasForeignKey(user => user.RoleId);
        
        // Связь User -> Education (1 ко многим)
        builder.Entity<EducationEntity>()
            .HasOne(e => e.User)
            .WithMany(u => u.Educations)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь User -> Experience (1 ко многим)
        builder.Entity<ExperienceEntity>()
            .HasOne(e => e.User)
            .WithMany(u => u.Experiences)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь User -> Stack (1 ко многим)
        builder.Entity<StackEntity>()
            .HasOne(s => s.User)
            .WithMany(u => u.Stacks)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Связь Respond -> User
        builder.Entity<RespondEntity>()
            .HasOne(r => r.Responder) 
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Связь Respond -> Project
        builder.Entity<RespondEntity>()
            .HasOne(r => r.Project)
            .WithMany()
            .HasForeignKey(r => r.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        
        #endregion

        #region Настройка связывающих таблиц

        builder.Entity<UserProject>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId);
            entity.HasOne(e => e.User)
                .WithMany(user => user.UserProjects)
                .HasForeignKey(e => e.UserId);
        });
        
        

        #endregion
    }
}