using System;
using System.ComponentModel.DataAnnotations;

namespace MVCCRUD.Models
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public long Salary { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Department { get; set; }
    }
}
