using PetPortalDAL.Entities.LinkingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Entities
{
    public class FrameworkEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProjectFramework> ProjectFrameworks { get; set; } = new List<ProjectFramework>();
    }
}
