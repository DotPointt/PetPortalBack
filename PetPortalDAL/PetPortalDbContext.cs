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

    public DbSet<ResetPasswordTokenEntity> ResetPasswordTokens { get; set; }

    /// <summary>
    /// Пользователи в базе данных.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Роли пользователей в базе данных.
    /// </summary>
    public DbSet<RoleEntity> Roles { get; set; }
    
    /// <summary>
    /// Связь пользователей и проектов.
    /// </summary>
    public DbSet<UserProject> UserProjects { get; set; }

    /// <summary>
    /// Теги в базе данных.
    /// </summary>
    public DbSet<TagEntity> Tags { get; set; }

    /// <summary>
    /// Связующая таблица между проектами и тегами (фреймворками).
    /// </summary>
    public DbSet<ProjectTag> ProjectTags { get; set; }
    
    /// <summary>
    /// Сообщения в чатах.
    /// </summary>
    public DbSet<ChatMessageEntity> ChatMessages { get; set; }

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