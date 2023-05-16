using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Employee
    {

        public int Id { get; set; }

        [Display(Name ="Full Name")]
        [Required(ErrorMessage = "You have to provide a valid full name")]
        [MinLength(12,ErrorMessage = "Full name mustn't be less than 12 charcters")]
        [MaxLength(50, ErrorMessage ="full name mustn't exceed 50 charcters")]
        public string FullName { get; set; }

        [DisplayName("Occupation")]
        [Required(ErrorMessage ="You have to privde a valid position")]
        [MinLength(2,ErrorMessage="Position mustn't be less than 2")]
        [MaxLength(50,ErrorMessage= "Position mustn't be more than 50")]
        public string Position { get; set; }
        
        [Range(2500,25000,ErrorMessage="Salary must be 2500 EGP:25000 EGP")]
        public double Salary { get; set; }

        [Range(1000,double.MaxValue, ErrorMessage ="Bonus mustn't be less than 1000")]
        public double Bonus { get; set; }

        [DisplayName("Phone")]
        [RegularExpression("^0\\d{10}$", ErrorMessage ="Invalid Phone Number!")]
        public string PhoneNo { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage ="Invalid Email Address!")]
        public string Email { get; set; }

        [NotMapped]
        [Compare("Email", ErrorMessage ="Email is not correct")]
        public string ConfirmEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password",ErrorMessage ="Password is not correct")]
        public string ConfirmPassword { get; set; } 

        public DateTime HiringDateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime AttendanceTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime LeavingTime { get; set; }

        [ValidateNever]
        public DateTime CreatedAt { get; set; }

        [ValidateNever]
        public DateTime LastUpdatedAt { get; set; }

        [DisplayName("Department")]
        [Range(1, int.MaxValue , ErrorMessage = "Choose a valid department")]
        public int DepartmentId { get; set; }

        [ValidateNever]
        public Department Department { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
