using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [MinLength(5, ErrorMessage = "Address mustn't be less than 5 charcters.")]
        [MaxLength(50,ErrorMessage ="Address mustn't be more than 50 charcters.")]
        public string Name { get; set; }

       
        public string Address { get; set; }



    }
}
