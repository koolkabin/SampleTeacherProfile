using CollegeWebsiteAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeWebsiteAdmin.Controllers
{
    public class TodoAppController : Controller
    {
        private readonly MyDBContext _context;
        public TodoAppController(MyDBContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AjaxAdd(string SubjectName)
        {
            try
            {
                //code for saving data to database
                Subject s1 = new Subject();
                s1.SubjectName = SubjectName;

                _context.Add(s1);

                _context.SaveChanges();
                return Json(new { status = true });

            }
            catch (Exception ex)
            {

                return Json(new { status = false, err = ex.Message });
            }


        }


        public IActionResult AjaxList()
        {
            try
            {
                IList<Subject> DataList = _context.Subjects.ToList();
                return Json(DataList);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, err = ex.Message });
            }


        }
        public IActionResult AjaxDelete(int id)
        {
            try
            {
                Subject OldRecrd = _context.Subjects.Find(id);
                if (OldRecrd == null)
                {
                    return Json(new { status = false, msg = "Record not found" });
                }


                _context.Subjects.Remove(OldRecrd);
                _context.SaveChanges();
                return Json(new { status = true });

            }
            catch (Exception ex)
            {
                return Json(new { status = false, err = ex.Message });
            }


        }
    }
}
