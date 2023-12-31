﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeWebsiteAdmin.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string SubjectName { get; set; }

        public virtual ICollection<TeacherSubjects> TeacherSubjects { get;set; }
        public Subject()
        {
            TeacherSubjects = new HashSet<TeacherSubjects>();
        }

    }




}
