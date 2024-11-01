using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DbContext _context;

        public ProjectsRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Get()
        {
            var projectEntities = await _context.Projects
                .AsNoTracking()
                .ToListAsync();

            var projects = projectEntities
                .Select(p => Project.Create(p.Id, p.Name, p.Description).project)
                .ToList();

            return projects;
        }

        public async Task<Guid> Create(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string description)
        {
            await _context.Projects
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.Name, p => name)
                    .SetProperty(p => p.Description, p => description));

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Projects
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
