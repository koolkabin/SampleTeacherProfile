using System.ComponentModel.DataAnnotations;

namespace CollegeWebsiteAdmin.Models
{
    public class CollegeTeachers
    {
        [Key]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int CollegeId { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime{ get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual Colleges Colleges { get; set; }
    }
}
