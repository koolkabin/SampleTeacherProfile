using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeWebsiteAdmin.Models
{
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string TeacherName { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string Telephone { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string ProfilePhotoName { get; set; }
        [NotMapped]
        public IFormFile UploadedPhoto { get; set; }

    }




}
