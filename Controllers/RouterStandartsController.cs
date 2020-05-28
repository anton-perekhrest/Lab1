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
    public class RouterStandartsController : Controller
    {
        private readonly RouterBaseContext _context;

        public RouterStandartsController(RouterBaseContext context)
        {
            _context = context;
        }

        // GET: RouterStandarts
        public async Task<IActionResult> Index(int? id)
        {
            if(id!=null)
            {
                var routerBaseContext1 = _context.RouterStandart.Where(r=>r.RouterId==id).Include(r => r.Router).Include(r => r.Standart);
                return View(await routerBaseContext1.ToListAsync());
            }
            var routerBaseContext = _context.RouterStandart.Include(r => r.Router).Include(r => r.Standart);
            return View(await routerBaseContext.ToListAsync());
        }

        // GET: RouterStandarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routerStandart = await _context.RouterStandart
                .Include(r => r.Router)
                .Include(r => r.Standart)
                .FirstOrDefaultAsync(m => m.RouterStandartId == id);
            if (routerStandart == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Routers", new { id = routerStandart.RouterId });
        }

        // GET: RouterStandarts/Create
        public IActionResult Create()
        {
            ViewData["RouterId"] = new SelectList(_context.Router, "RouterId", "Name");
            ViewData["StandartId"] = new SelectList(_context.Standart, "StandartId", "Standart1");
            return View();
        }

        // POST: RouterStandarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouterStandartId,RouterId,StandartId")] RouterStandart routerStandart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(routerStandart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouterId"] = new SelectList(_context.Router, "RouterId", "Name", routerStandart.RouterId);
            ViewData["StandartId"] = new SelectList(_context.Standart, "StandartId", "Standart1", routerStandart.StandartId);
            return View(routerStandart);
        }

        // GET: RouterStandarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routerStandart = await _context.RouterStandart.FindAsync(id);
            if (routerStandart == null)
            {
                return NotFound();
            }
            ViewData["RouterId"] = new SelectList(_context.Router, "RouterId", "Name", routerStandart.RouterId);
            ViewData["StandartId"] = new SelectList(_context.Standart, "StandartId", "Standart1", routerStandart.StandartId);
            return View(routerStandart);
        }

        // POST: RouterStandarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouterStandartId,RouterId,StandartId")] RouterStandart routerStandart)
        {
            if (id != routerStandart.RouterStandartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(routerStandart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouterStandartExists(routerStandart.RouterStandartId))
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
            ViewData["RouterId"] = new SelectList(_context.Router, "RouterId", "Name", routerStandart.RouterId);
            ViewData["StandartId"] = new SelectList(_context.Standart, "StandartId", "Standart1", routerStandart.StandartId);
            return View(routerStandart);
        }

        // GET: RouterStandarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routerStandart = await _context.RouterStandart
                .Include(r => r.Router)
                .Include(r => r.Standart)
                .FirstOrDefaultAsync(m => m.RouterStandartId == id);
            if (routerStandart == null)
            {
                return NotFound();
            }

            return View(routerStandart);
        }

        // POST: RouterStandarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routerStandart = await _context.RouterStandart.FindAsync(id);
            _context.RouterStandart.Remove(routerStandart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouterStandartExists(int id)
        {
            return _context.RouterStandart.Any(e => e.RouterStandartId == id);
        }
    }
}
