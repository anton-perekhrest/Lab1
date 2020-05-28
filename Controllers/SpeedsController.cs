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
    public class SpeedsController : Controller
    {
        private readonly RouterBaseContext _context;

        public SpeedsController(RouterBaseContext context)
        {
            _context = context;
        }

        // GET: Speeds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Speed.ToListAsync());
        }

        // GET: Speeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speed = await _context.Speed
                .FirstOrDefaultAsync(m => m.SpeedId == id);
            if (speed == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Routers", new { SpId=id });
        }

        // GET: Speeds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Speeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpeedId,Speed1")] Speed speed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(speed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speed);
        }

        // GET: Speeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speed = await _context.Speed.FindAsync(id);
            if (speed == null)
            {
                return NotFound();
            }
            return View(speed);
        }

        // POST: Speeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpeedId,Speed1")] Speed speed)
        {
            if (id != speed.SpeedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeedExists(speed.SpeedId))
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
            return View(speed);
        }

        // GET: Speeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speed = await _context.Speed
                .FirstOrDefaultAsync(m => m.SpeedId == id);
            if (speed == null)
            {
                return NotFound();
            }

            return View(speed);
        }

        // POST: Speeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routers = _context.Router.Where(c => c.SpeedId == id).Include(c => c.Price).Include(c => c.Diapason).Include(c => c.Speed).ToList();
            foreach (var c in routers)
            {
                var routSt = _context.RouterStandart.Where(d => d.RouterId == c.RouterId).Include(d => d.Router).Include(d => d.Standart).ToList();
                _context.RouterStandart.RemoveRange(routSt);
                await _context.SaveChangesAsync();
            }
            _context.Router.RemoveRange(routers);
            await _context.SaveChangesAsync();
            var speed = await _context.Speed.FindAsync(id);
            _context.Speed.Remove(speed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeedExists(int id)
        {
            return _context.Speed.Any(e => e.SpeedId == id);
        }
    }
}
