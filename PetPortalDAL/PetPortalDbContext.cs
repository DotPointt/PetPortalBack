using Microsoft.EntityFrameworkCore;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

namespace PetPortalDAL
{
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
    }
}