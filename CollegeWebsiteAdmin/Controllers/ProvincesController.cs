using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeWebsiteAdmin.Models;
using CollegeWebsiteAdmin.Extensions;
using Microsoft.Extensions.Logging.Abstractions;

namespace CollegeWebsiteAdmin.Controllers
{
    public class ProvincesController : Controller
    {
        private readonly MyDBContext _context;

        public ProvincesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Provinces
        public async Task<IActionResult> Index()
        {
            Teacher SessionValue = HttpContext.Session.Get<Teacher>("LoggedInUser");

            if (SessionValue == null)
            {
                //
                return RedirectToAction("Login", "Home");
            }

            return _context.Province != null ?
                        View(await _context.Province.ToListAsync()) :
                        Problem("Entity set 'MyDBContext.Province'  is null.");
        }

        // GET: Provinces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Province == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .FirstOrDefaultAsync(m => m.Id == id);
            if (province == null)
            {
                return NotFound();
            }

            //for explicit loading;
            _context.Entry(province).Collection(x => x.District).Load();

            ////normal db context/table ma where condition apply
            //IList<District> RelatedDisrct = _context.District
            //    // Lambda expression
            //    .Where(x => x.ProvinceId == id)
            //    .ToList();

            ////manually override values
            //province.District = RelatedDisrct;

            ////where condition will be auto appilied by -> navigation property
            //IList<District> RelDist = province.District.ToList();


            //Eger Loading or explicint loading
            //Lazy loading -> discard
            return View(province);
        }

        // GET: Provinces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Provinces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProvinceName")] Province province)
        {
            if (ModelState.IsValid)
            {
                _context.Add(province);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(province);
        }

        // GET: Provinces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Province == null)
            {
                return NotFound();
            }

            var province = await _context.Province.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            return View(province);
        }

        // POST: Provinces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProvinceName")] Province province)
        {
            if (id != province.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(province);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvinceExists(province.Id))
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
            return View(province);
        }

        // GET: Provinces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Province == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .FirstOrDefaultAsync(m => m.Id == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // POST: Provinces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Province == null)
            {
                return Problem("Entity set 'MyDBContext.Province'  is null.");
            }
            var province = await _context.Province.FindAsync(id);
            if (province != null)
            {
                _context.Province.Remove(province);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvinceExists(int id)
        {
            return (_context.Province?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
