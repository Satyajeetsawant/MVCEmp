using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeDapper.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        /*[MinLength(4)]*/
        [MaxLength(50)]
        public string City { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        
    }
}