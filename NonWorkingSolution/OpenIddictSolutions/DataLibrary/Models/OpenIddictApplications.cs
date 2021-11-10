using OpenIddict.EntityFrameworkCore.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    [NotMapped]
    public partial class OpenIddictApplications : OpenIddictEntityFrameworkCoreApplication
    {
    }
}