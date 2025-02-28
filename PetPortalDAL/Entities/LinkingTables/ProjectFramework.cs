using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Entities.LinkingTables
{
    public class ProjectFramework
    {
        public Guid ProjectId { get; set; }
        public ProjectEntity Project { get; set; }

        public Guid FrameworkId { get; set; }
        public FrameworkEntity Framework { get; set; }
    }
}
