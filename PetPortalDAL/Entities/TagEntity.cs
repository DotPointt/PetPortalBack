using PetPortalDAL.Entities.LinkingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Entities
{
    public class TagEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
    }
}
