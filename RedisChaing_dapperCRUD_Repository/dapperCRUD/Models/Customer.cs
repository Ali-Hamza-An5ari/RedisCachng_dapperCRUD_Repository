using dapperCRUD.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dapperCRUD.Models
{
    public class Customer
    {
        [Key]
        [Required]
        [BindNever]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set ; }= Guid.NewGuid();


        [Required(ErrorMessage = "name required")]
        [MinLength(2)]
        [MaxLength(255)]
        public string Name { get; set; }


        [Required, EmailAddress]
        public string Email { get; set; }


        
        [DataType(DataType.Date)]
        [CustomerDateOfBirthValidation]
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;
    }

}
