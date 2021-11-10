using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public partial class ApplicationRoles : IdentityRole<Guid>
    {
        public ApplicationRoles()
        {
            ApplicationUserRoles = new HashSet<ApplicationUserRoles>();
        }
        //public DateTime SysStart { get; set; }
        //public DateTime SysEnd { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ApplicationUserRoles> ApplicationUserRoles { get; set; }       
    }
}