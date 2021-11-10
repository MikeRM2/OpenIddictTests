using DataLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLibrary.IdentityManagers
{
    public class ApplicationRoleManager : RoleManager<ApplicationRoles>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRoles> store, IEnumerable<IRoleValidator<ApplicationRoles>> roleValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRoles>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {            
        }
    }
}
