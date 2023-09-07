using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeWebsiteAdmin.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CollegeWebsiteAdmin.Extensions;
using CollegeWebsiteAdmin.ViewModels;

namespace CollegeWebsiteAdmin.Controllers
{
    public class TeachersController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _MyEnvVariable;

        public TeachersController(MyDBContext context, IWebHostEnvironment myEnvVariable)
        {
            _context = context;
            _MyEnvVariable = myEnvVariable;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            Teacher SessionValue = HttpContext.Session.Get<Teacher>("LoggedInUser");

            if (SessionValue == null)
            {
                //
                return RedirectToAction("Login", "Home");
            }

            IQueryable<Teacher> _Que = _context.Teachers;

            int[] ids = new int[] { 5, 2, 3 };
            IList<VMTeacherOutput> DataList1 = _Que
                .Where(x => ids.Contains(x.Id))
                .Select(x => new VMTeacherOutput()
                {
                    Id = x.Id,
                    Title = x.TeacherName
                })
                .ToList();
            int teacherCount = DataList1.Count(); //already loaded list data -> count
            teacherCount = _Que.Count(); //makes database action qeuery and get count only from db

            IDictionary<int, string> DataList = _context.Teachers //iqueryable of teachers
                                                                  //.ToList()
                .Where(x => x.TeacherName.Contains("ram"))
                //.OrderBy(x => x.TeacherName)
                .Select(x => new VMTeacherOutput()
                {
                    Id = x.Id,
                    Title = x.TeacherName
                })
                .OrderBy(x => x.Title)
                .Take(5)
                .Skip(100)
                //chaining
                //.ToList();=
                .ToDictionary(x => x.Id, x => x.Title);

            decimal TotalSalary = _context.Teachers
                .Where(x => x.Salary > 0)
                .Sum(x => x.Salary);

            IList<VMTeacherOutput2> DataList2 = _context.Teachers
                .Select(x => new VMTeacherOutput2()
                {
                    Id = x.Id,
                    Title = x.TeacherName,
                    Salary = x.Salary,
                    TotalSalary = TotalSalary
                })
                .ToList();

            IList<VMTeacherOutput2> DataList3 = _context.Teachers
                .GroupBy(x => x.SubjectId)
                .Select(x => new VMTeacherOutput2()
                {
                    Id = x.FirstOrDefault().Id,
                    Title = x.FirstOrDefault().TeacherName,
                    Salary = x.FirstOrDefault().Salary,
                    TotalSalary = x.Sum(x => x.Salary)
                })
                .ToList();




            return _context.Teachers != null ?
                        View(await _context.Teachers.ToListAsync()) :
                        Problem("Entity set 'MyDBContext.Teachers'  is null.");
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        //no attributes of method i.e default get method will be used
        public IActionResult Create()
        {
            //List of subjects get and pass to view
            IList<Subject> subJectList = _context.Subjects.ToList();
            ViewData["SubjectList"] = subJectList;
            _AddEditCommon();

            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,TeacherName,Address,Telephone,Email,UploadedPhoto")] VMTeacherInput Data)
        {
            string fileName = await UploadHelper(Data);
            Data.ProfilePhotoName = fileName;

            #region Revalidation of user given Data
            ModelState.Clear();
            TryValidateModel(Data);
            #endregion

            if (ModelState.IsValid)
            {
                _context.Add(Data);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _AddEditCommon();
            return View(Data);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            _AddEditCommon();

            _context.Entry(teacher).Collection(x => x.TeacherSubjects).Load();

            return View(teacher);
        }

        private void _AddEditCommon()
        {
            //List of subjects get and pass to view
            IList<Subject> subJectList = _context.Subjects.ToList();
            ViewData["SubjectList"] = subJectList;

            //List of subjects get and pass to view
            IList<Colleges> CollegeList = _context.Colleges.ToList();
            ViewData["CollegeList"] = CollegeList;
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherName,Address,Telephone,Email,UploadedPhoto")] VMTeacherInput Data)
        {
            if (id != Data.Id)
            {
                return NotFound();
            }
            string fileName = await UploadHelper(Data);
            Data.ProfilePhotoName = fileName;

            #region Revalidation of user given Data
            ModelState.Clear();
            TryValidateModel(Data);
            #endregion


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(Data.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _AddEditCommon();
            return View(Data);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'MyDBContext.Teachers'  is null.");
            }
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Home", new { id = 123 });
        }

        private bool TeacherExists(int id)
        {
            return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<string> UploadHelper(VMTeacherInput colleges)
        {
            #region For File Upload Process

            //File UPload 
            string fileName = colleges.UploadedPhoto.FileName;

            string destinationPath = Path.Combine(_MyEnvVariable.WebRootPath, "private/teacher");

            //if folder exists or not check 
            //if no then we can cretae it also
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            string filePath = Path.Combine(destinationPath, fileName);

            // Save the uploaded file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await colleges.UploadedPhoto.CopyToAsync(stream);
            }
            #endregion
            return fileName;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMatching(
            [Bind("Id,TeacherName,Address,Telephone,Email,UploadedPhoto, Password")] VMTeacherInput t1
            , int[] selSubject, IList<CollegeTeachers> CollegeTeacherData)
        {

            string fileName = await UploadHelper(t1);
            t1.ProfilePhotoName = fileName;
            t1.Address = "NA";
            t1.TeacherName = "asads";
            t1.Email = "asdas@gmail.com";
            t1.Telephone = "123123";
            #region Revalidation of user given Data
            ModelState.Clear();
            TryValidateModel(t1);
            #endregion

            if (ModelState.IsValid)
            {
                t1.Password = t1.Password.EncryptSha256();
                //gives list of subjects or records of teachersubjects table matching teacher id
                IList<TeacherSubjects> TeacherSubject = _context.TeacherSubjects.Where(x => x.TeacherID == t1.Id).ToList();

                _context.TeacherSubjects.RemoveRange(TeacherSubject);

                foreach (var item in selSubject)
                {
                    var rec = new TeacherSubjects()
                    {
                        //TeacherID = //auto generate wala ho.. confusion
                        SubjectID = item
                    };

                    t1.TeacherSubjects.Add(rec);
                }


                //Gets old list from db
                IList<CollegeTeachers> CollegeTeachers = _context.CollegeTeachers
                    .Where(x => x.TeacherId == t1.Id).ToList();

                //removes those list from db
                _context.CollegeTeachers.RemoveRange(CollegeTeachers);

                foreach (var item in CollegeTeacherData)
                {
                    var rec = new CollegeTeachers()
                    {
                        //TeacherID = //auto generate wala ho.. confusion
                        CollegeId = item.CollegeId,
                        FromTime = item.FromTime,
                        ToTime = item.ToTime
                    };

                    t1.CollegeTeachers.Add(rec);
                }
                _context.Add(t1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _AddEditCommon();
            return View(t1);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateMatching(
        //  [Bind("Id,TeacherName,Address,Telephone,Email,UploadedPhoto")] Teacher t1
        //  , CollegeTeachers[] CollegeTeacherData)
        //{

        //    string fileName = await UploadHelper(t1);
        //    t1.ProfilePhotoName = fileName;
        //    t1.Address = "NA";
        //    t1.TeacherName = "asads";
        //    t1.Email = "asdas@gmail.com";
        //    t1.Telephone = "123123";
        //    #region Revalidation of user given Data
        //    ModelState.Clear();
        //    TryValidateModel(t1);
        //    #endregion

        //    if (ModelState.IsValid)
        //    {
        //        foreach (var item in selSubject)
        //        {
        //            var rec = new TeacherSubjects()
        //            {
        //                //TeacherID = //auto generate wala ho.. confusion
        //                SubjectID = item
        //            };

        //            t1.TeacherSubjects.Add(rec);
        //        }

        //        _context.Add(t1);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(t1);
        //}

    }
}
