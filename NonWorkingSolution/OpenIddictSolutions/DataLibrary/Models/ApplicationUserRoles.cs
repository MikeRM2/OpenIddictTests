using Microsoft.AspNetCore.Identity;
using System;

namespace DataLibrary.Models
{
    public partial class ApplicationUserRoles : IdentityUserRole<Guid>
    {
        public bool IsActive { get; set; }
        //public DateTime SysStart { get; set; }
        //public DateTime SysEnd { get; set; }

        public virtual ApplicationRoles Role { get; set; }
        public virtual ApplicationUsers User { get; set; }
    }
}