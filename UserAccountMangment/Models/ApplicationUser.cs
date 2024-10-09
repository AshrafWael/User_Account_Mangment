//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagmentSystem.DAL.Data.Models
{
    public class ApplicationUser :IdentityUser
    {
        
        public string FirstName { get; set; }
        public string lasttName { get; set; }

    }
}
