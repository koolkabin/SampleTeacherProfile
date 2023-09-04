using CollegeWebsiteAdmin.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeWebsiteAdmin.ViewModels
{
    public class VMTeacherInput : Teacher
    {
        [NotMapped]
        public IFormFile UploadedPhoto { get; set; }
    }
}
