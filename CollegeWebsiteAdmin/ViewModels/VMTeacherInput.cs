using CollegeWebsiteAdmin.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeWebsiteAdmin.ViewModels
{
    public class VMTeacherInput : Teacher
    {
        [NotMapped]
        public IFormFile UploadedPhoto { get; set; }
    }

    public class VMTeacherOutput
    {
        public int Id { get; set; }
        public string Title { get; set; }

    }
    public class VMTeacherOutput2
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public decimal Salary { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal MyPercent => TotalSalary == 0 ? 0 : (Salary * 100 / TotalSalary);

    }
}
