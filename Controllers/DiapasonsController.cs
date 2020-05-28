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
    public class DiapasonsController : Controller
    {
        private readonly RouterBaseContext _context;

        public DiapasonsController(RouterBaseContext context)
        {
            _context = context;
        }

        // GET: Diapasons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Diapason.ToListAsync());
        }

        // GET: Diapasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diapason = await _context.Diapason
                .FirstOrDefaultAsync(m => m.DiapasonId == id);
            if (diapason == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Routers", new { dId = id });
        }

        // GET: Diapasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diapasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiapasonId,Diapason1")] Diapason diapason)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diapason);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diapason);
        }

        // GET: Diapasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diapason = await _context.Diapason.FindAsync(id);
            if (diapason == null)
            {
                return NotFound();
            }
            return View(diapason);
        }

        // POST: Diapasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiapasonId,Diapason1")] Diapason diapason)
        {
            if (id != diapason.DiapasonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diapason);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiapasonExists(diapason.DiapasonId))
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
            return View(diapason);
        }

        // GET: Diapasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diapason = await _context.Diapason
                .FirstOrDefaultAsync(m => m.DiapasonId == id);
            if (diapason == null)
            {
                return NotFound();
            }

            return View(diapason);
        }

        // POST: Diapasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routers = _context.Router.Where(c => c.DiapasonId == id).Include(c => c.Price).Include(c => c.Diapason).Include(c => c.Speed).ToList();
            foreach (var c in routers)
            {
                var routSt = _context.RouterStandart.Where(d => d.RouterId == c.RouterId).Include(d => d.Router).Include(d => d.Standart).ToList();
                _context.RouterStandart.RemoveRange(routSt);
                await _context.SaveChangesAsync();
            }
            _context.Router.RemoveRange(routers);
            var diapason = await _context.Diapason.FindAsync(id);
            _context.Diapason.Remove(diapason);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiapasonExists(int id)
        {
            return _context.Diapason.Any(e => e.DiapasonId == id);
        }
    }
}
