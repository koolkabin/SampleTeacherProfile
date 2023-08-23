using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeWebsiteAdmin.Models;
using Microsoft.AspNetCore.Hosting;

namespace CollegeWebsiteAdmin.Controllers
{
    public class CollegesController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _MyEnvVariable;

        public CollegesController(MyDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _MyEnvVariable = webHostEnvironment;
        }

        // GET: Colleges
        public async Task<IActionResult> Index()
        {
            return _context.Colleges != null ?
                        View(await _context.Colleges.ToListAsync()) :
                        Problem("Entity set 'MyDBContext.Colleges'  is null.");
        }

        // GET: Colleges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Colleges == null)
            {
                return NotFound();
            }

            var colleges = await _context.Colleges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colleges == null)
            {
                return NotFound();
            }

            return View(colleges);
        }

        // GET: Colleges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Colleges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,CollegeName,Address,Telephone,Email,Website,UploadedPhoto")] Colleges Data)
        {
            string fileName = await UploadHelper(Data);
            Data.LogoFile = fileName;

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

        private async Task<string> UploadHelper(Colleges colleges)
        {
            #region For File Upload Process

            //File UPload 
            if (colleges.UploadedPhoto == null)
            {
                return "N/A";
            }
            string fileName = colleges.UploadedPhoto.FileName;

            string destinationPath = Path.Combine(_MyEnvVariable.WebRootPath, "private/college/");

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

        // GET: Colleges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Colleges == null)
            {
                return NotFound();
            }

            var colleges = await _context.Colleges.FindAsync(id);
            if (colleges == null)
            {
                return NotFound();
            }
            return View(colleges);
        }

        // POST: Colleges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CollegeName,Address,Telephone,Email,Website,UploadedPhoto")] Colleges Data)
        {
            if (id != Data.Id)
            {
                return NotFound();
            }

            string fileName = await UploadHelper(Data);
            Data.LogoFile = fileName;

            #region Revalidation of user given Data
            ModelState.Clear();
            TryValidateModel(Data);
            #endregion


            if (ModelState.IsValid)
            {
                try
                {
                    Colleges oldData = _context.Colleges.Find(id);
                    if (oldData == null)
                    {
                        throw new Exception("Invalid Old Data Ref ID");
                    }
                    oldData.CollegeName = Data.CollegeName;
                    oldData.Address = Data.Address;


                    _context.Update(oldData);
                    //overwrites all column data of selected matching primary key record
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollegesExists(Data.Id))
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

        // GET: Colleges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Colleges == null)
            {
                return NotFound();
            }

            var colleges = await _context.Colleges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colleges == null)
            {
                return NotFound();
            }

            return View(colleges);
        }

        // POST: Colleges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Colleges == null)
            {
                return Problem("Entity set 'MyDBContext.Colleges'  is null.");
            }
            var colleges = await _context.Colleges.FindAsync(id);
            if (colleges != null)
            {
                _context.Colleges.Remove(colleges);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollegesExists(int id)
        {
            return (_context.Colleges?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // POST: Colleges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyOwnCreate(
            [Bind("Id,CollegeName,Address,Telephone,Email,Website,UploadedPhoto")] Colleges c1)
        {
            string fileName = await UploadHelper(c1);
            c1.LogoFile = fileName;

            #region Revalidation of user given Data
            ModelState.Clear();
            TryValidateModel(c1);
            #endregion

            if (ModelState.IsValid)
            {
                _context.Add(c1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c1);
        }

        //method 1 for manual catching
        [HttpPost]
        public async Task<IActionResult> NewCollege(string CollegeName, string Address, string Telephone)
        {

            //code for inserting data to datatabse
            Teacher t1 = new Teacher()
            {
                TeacherName = CollegeName,
                Address = Address,
                Telephone = Telephone,
                Email = "unknownemail@gmail.com",
                ProfilePhotoName = "N/A"
            };

            //shortcut way to insert data in related table
            _context.Add(t1);

            //long way  to insert data in related table
            _context.Add<Teacher>(t1);

            //make changes to db in permanent way
            _context.SaveChanges();





            return Content("Done");// new JsonContent(new { status=true, msg="Data Saved Succesfully"});
        }

        //method 2 for manual catching
        [HttpPost]
        public async Task<IActionResult> NewCollegeClass(Colleges Data)
        {
            return View();
        }

    }
}
