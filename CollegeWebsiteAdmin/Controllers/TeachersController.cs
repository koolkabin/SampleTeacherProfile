using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeWebsiteAdmin.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,TeacherName,Address,Telephone,Email,UploadedPhoto")] Teacher Data)
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
            //List of subjects get and pass to view
            IList<Subject> subJectList = _context.Subjects.ToList();
            ViewData["SubjectList"] = subJectList;
            
            //List of subjects get and pass to view
            IList<Colleges> CollegeList = _context.Colleges.ToList();
            ViewData["CollegeList"] = CollegeList;

            _context.Entry(teacher).Collection(x => x.TeacherSubjects).Load();

            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherName,Address,Telephone,Email,UploadedPhoto")] Teacher Data)
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

        private async Task<string> UploadHelper(Teacher colleges)
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
            [Bind("Id,TeacherName,Address,Telephone,Email,UploadedPhoto")] Teacher t1
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
                foreach (var item in selSubject)
                {
                    var rec = new TeacherSubjects()
                    {
                        //TeacherID = //auto generate wala ho.. confusion
                        SubjectID = item
                    };

                    t1.TeacherSubjects.Add(rec);
                }

                foreach (var item in CollegeTeacherData)
                {
                    var rec = new CollegeTeachers()
                    {
                        //TeacherID = //auto generate wala ho.. confusion
                        CollegeId = item.CollegeId,
                        FromTime = item.FromTime,
                        ToTime=item.ToTime
                    };

                    t1.CollegeTeachers.Add(rec);
                }
                _context.Add(t1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
