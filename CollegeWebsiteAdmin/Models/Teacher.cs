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

        //navigation property from parent to child
        //in case of 1 to 1 relation: direct property only
        //in case of 1 to many relation: collection property
        //it won't be stored in db
        public virtual ICollection<TeacherSubjects> TeacherSubjects { get; set; }
        public Teacher()
        {
            TeacherSubjects = new HashSet<TeacherSubjects>();
        }

    }

    public class TeacherSubjects
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Teachers")]
        //method data annotation method
        public int TeacherID { get; set; } //assumes as 1 to many relationship
        public int SubjectID { get; set; }//assumes as 1 to many relationship

        //child to parent navigation property 
        public virtual Teacher Teacher { get; set; }

        //support
        //navigation property
    }


}
