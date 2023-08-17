using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeWebsiteAdmin.Models
{
    public class Colleges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string CollegeName { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string Telephone{ get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string Website{ get; set; }
        [MaxLength(255)]
        public string LogoFile { get; set; }
        [NotMapped]
        public IFormFile UploadedPhoto { get; set; }

    }




}
