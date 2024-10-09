using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagmentSystem.BLL.Dtos.AccountDtos
{
    public class AccountRegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string lasttName { get; set; }
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare("Password")]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        
    }
}
