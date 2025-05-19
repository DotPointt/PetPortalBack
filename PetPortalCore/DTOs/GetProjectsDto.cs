using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalCore.DTOs
{
    public class GetProjectsDto
    {
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto> { };
        public int ProjectsCount { get; set; }
    }
}
