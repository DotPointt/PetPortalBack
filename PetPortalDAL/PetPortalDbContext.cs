using Microsoft.EntityFrameworkCore;
using PetPortalDAL.Configurations;
using PetPortalDAL.Entities;

namespace PetPortalDAL;

/// <summary>
/// Data base context.
/// </summary>
public class  PetPortalDbContext : DbContext
{
    /// <summary>
    /// Context constructor.
    /// </summary>
    /// <param name="options"></param>
    public PetPortalDbContext(DbContextOptions<PetPortalDbContext> options)
        : base(options)
    {
    }
        
    # region Db sets
    
    /// <summary>
    /// Data base projects.
    /// </summary>
    public DbSet<ProjectEntity> Projects { get; set; }
        
    /// <summary>
    /// Database users.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Database roles.
    /// </summary>
    public DbSet<RoleEntity> Roles { get; set; }
    
    /// <summary>
    /// Database user-projects.
    /// </summary>
    public DbSet<UserProject> UserProjects { get; set; }
    
    # endregion
    
    /// <summary>
    /// Models configuring.
    /// </summary>
    /// <param name="builder">Model builder.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Models configuration application
            
        builder.ApplyConfiguration(new ProjectConfigurations());
        builder.ApplyConfiguration(new UserConfigurations());
        builder.ApplyConfiguration(new RoleConfigurations());
            
        #endregion

        #region Setting up model links
        
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

        #region Setting up linking tables

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