using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouterLab;

namespace RouterLab.Controllers
{
    public class StandartsController : Controller
    {
        private readonly RouterBaseContext _context;

        public StandartsController(RouterBaseContext context)
        {
            _context = context;
        }

        // GET: Standarts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Standart.ToListAsync());
        }

        // GET: Standarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standart = await _context.Standart
                .FirstOrDefaultAsync(m => m.StandartId == id);
            if (standart == null)
            {
                return NotFound();
            }

            return View(standart);
        }

        // GET: Standarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Standarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StandartId,Standart1")] Standart standart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(standart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(standart);
        }

        // GET: Standarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standart = await _context.Standart.FindAsync(id);
            if (standart == null)
            {
                return NotFound();
            }
            return View(standart);
        }

        // POST: Standarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StandartId,Standart1")] Standart standart)
        {
            if (id != standart.StandartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(standart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StandartExists(standart.StandartId))
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
            return View(standart);
        }

        // GET: Standarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standart = await _context.Standart
                .FirstOrDefaultAsync(m => m.StandartId == id);
            if (standart == null)
            {
                return NotFound();
            }

            return View(standart);
        }

        // POST: Standarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routSt = _context.RouterStandart.Where(d => d.StandartId == id).Include(d => d.Router).Include(d => d.Standart).ToList();
            _context.RouterStandart.RemoveRange(routSt);
            var standart = await _context.Standart.FindAsync(id);
            _context.Standart.Remove(standart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StandartExists(int id)
        {
            return _context.Standart.Any(e => e.StandartId == id);
        }
    }
}
