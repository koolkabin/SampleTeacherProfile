using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeWebsiteAdmin.Models
{
    public class Teacher
    {
        [Key] // this is prary key
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
        public string Password { get; set; }

        //navigation property from parent to child
        //in case of 1 to 1 relation: direct property only
        //in case of 1 to many relation: collection property
        //it won't be stored in db
        public virtual ICollection<TeacherSubjects> TeacherSubjects { get; set; }
        public virtual ICollection<CollegeTeachers> CollegeTeachers { get; set; }
        public Teacher()
        {
            TeacherSubjects = new HashSet<TeacherSubjects>();
            CollegeTeachers = new HashSet<CollegeTeachers>();
        }

    }

    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DistrictName { get; set; }
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
    }
    public class Province
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ProvinceName { get; set; }
        public virtual ICollection<District> District { get; set; }
        public Province()
        {
            District = new HashSet<District>();
        }

    }
}
