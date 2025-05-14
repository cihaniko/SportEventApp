using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsEventApp.Infrastructure.Dtos
{
    public class RegisterUserRequest
    {
        [Required]
        public string UserName { get; set; } 
    }
}
