using PetPortalCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalCore.Abstractions
{
    public interface IProjectsService
    {
        Task<List<Project>> GetAllProjects();
        Task<Guid> CreateProject(Project project);
        Task<Guid> UpdateProject(Guid id, string name, string description);
        Task<Guid> Delete(Guid id);
    }
}
