using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeWebsiteAdmin.Models
{
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
        public virtual Subject Subject { get; set; }

        //support
        //navigation property
    }


}
