using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Department
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "You have to provide a valid name")]
        [MinLength(2, ErrorMessage = "Name mustn't be less than 2 charcters")]
        [MaxLength(20, ErrorMessage = "Name mustn't exceed 20 charcters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "You have to privde a valid Description")]
        [MinLength(5, ErrorMessage = "Description mustn't be less than 5")]
        [MaxLength(50, ErrorMessage = "Description mustn't be more than 50")]
        public string Description { get; set; }

        [ValidateNever]
        public ICollection<Employee> Employees { get; set; }

    }
}
