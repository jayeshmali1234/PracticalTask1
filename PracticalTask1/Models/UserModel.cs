using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalTask1.Models
{
    public class UserModel
    { 
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is Required.....")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        [StringLength(10, ErrorMessage = "Phone must be 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
      
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password {  get; set; }
        public bool IsAdmin { get; set; }
    }
}