using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Entities.LinkingTables
{
    public class ProjectTag
    {
        /// <summary>
        /// ProjectId
        /// </summary>
        public Guid ProjectId { get; set; }
        public ProjectEntity Project { get; set; }

        public Guid TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}
