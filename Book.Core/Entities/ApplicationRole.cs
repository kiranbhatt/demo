using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Books.Core.Entities
{
    [Table("ApplicationRole")]
    public class ApplicationRole : IdentityRole
    {
    }
}
