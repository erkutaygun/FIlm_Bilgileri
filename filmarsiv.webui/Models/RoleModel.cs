using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using filmarsiv.webui.Identity;
using Microsoft.AspNetCore.Identity;

namespace filmarsiv.webui.Models
{
    public class RoleModel
    {   
        [Required]
        public string Name { get; set; }
    }

    public class RoleDetails{
        public IdentityRole Role { get; set;}

        public IEnumerable<User> Memebers { get; set;}
        public IEnumerable<User> NonMemebers { get; set;}

    }
    public class RoleEditModel{
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}