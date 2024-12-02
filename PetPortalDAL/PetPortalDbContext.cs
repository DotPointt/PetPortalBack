using Microsoft.EntityFrameworkCore;
using PetPortalCore.Models;
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
        
    /// <summary>
    /// Data base projects.
    /// </summary>
    public DbSet<ProjectEntity> Projects { get; set; }
        
    /// <summary>
    /// Data base users.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Models configuring.
    /// </summary>
    /// <param name="builder">Model builder.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Models configuration application
            
        builder.ApplyConfiguration(new ProjectConfigurations());
        builder.ApplyConfiguration(new UserConfigurations());
            
        #endregion

        #region Setting up model links

        builder.Entity<UserEntity>()
            .HasMany(user => user.Projects)
            .WithOne(project => project.User)
            .HasForeignKey(project => project.OwnerId)
            .IsRequired();
            
        builder.Entity<ProjectEntity>()
            .HasOne(project => project.User)
            .WithMany(user => user.Projects)
            .HasForeignKey(project => project.OwnerId)
            .IsRequired();


        #endregion
    }
}