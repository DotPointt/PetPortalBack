using PetPortalCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalCore.Abstractions
{
    public interface IProjectsRepository
    {
        Task<Guid> Create(Project project);
        Task<Guid> Delete(Guid id);
        Task<List<Project>> Get();
        Task<Guid> Update(Guid id, string name, string description);
    }
}
