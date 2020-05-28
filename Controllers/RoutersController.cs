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
    public class RoutersController : Controller
    {
        private readonly RouterBaseContext _context;

        public RoutersController(RouterBaseContext context)
        {
            _context = context;
        }

        // GET: Routers
        public async Task<IActionResult> Index(int id, int SpId , int dId, int prId)
        {
            if (id != 0)
            {
                var routerBaseContext1 = _context.Router.Where(r=>r.RouterId==id).Include(r => r.Diapason).Include(r => r.Price).Include(r => r.Speed);
                return View(await routerBaseContext1.ToListAsync());
            }
            if (SpId != 0)
            {
                var routerBaseContext1 = _context.Router.Where(r => r.SpeedId == SpId).Include(r => r.Diapason).Include(r => r.Price).Include(r => r.Speed);
                return View(await routerBaseContext1.ToListAsync());
            }
            if (dId != 0)
            {
                var routerBaseContext1 = _context.Router.Where(r => r.DiapasonId== dId).Include(r => r.Diapason).Include(r => r.Price).Include(r => r.Speed);
                return View(await routerBaseContext1.ToListAsync());
            }
            if (prId != 0)
            {
                var routerBaseContext1 = _context.Router.Where(r => r.PriceId == prId).Include(r => r.Diapason).Include(r => r.Price).Include(r => r.Speed);
                return View(await routerBaseContext1.ToListAsync());
            }
            var routerBaseContext = _context.Router.Include(r => r.Diapason).Include(r => r.Price).Include(r => r.Speed);
            return View(await routerBaseContext.ToListAsync());
        }

        // GET: Routers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var router = await _context.Router
                .Include(r => r.Diapason)
                .Include(r => r.Price)
                .Include(r => r.Speed)
                .FirstOrDefaultAsync(m => m.RouterId == id);
            if (router == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "RouterStandarts", new { id });
        }

        // GET: Routers/Create
        public IActionResult Create()
        {
            ViewData["DiapasonId"] = new SelectList(_context.Diapason, "DiapasonId", "Diapason1");
            ViewData["PriceId"] = new SelectList(_context.Price, "PriceId", "Price1");
            ViewData["SpeedId"] = new SelectList(_context.Speed, "SpeedId", "Speed1");
            return View();
        }

        // POST: Routers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouterId,Name,Year,PriceId,DiapasonId,SpeedId")] Router router)
        {
            if (ModelState.IsValid)
            {
                _context.Add(router);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiapasonId"] = new SelectList(_context.Diapason, "DiapasonId", "Diapason1", router.DiapasonId);
            ViewData["PriceId"] = new SelectList(_context.Price, "PriceId", "Price1", router.PriceId);
            ViewData["SpeedId"] = new SelectList(_context.Speed, "SpeedId", "Speed1", router.SpeedId);
            return View(router);
        }

        // GET: Routers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var router = await _context.Router.FindAsync(id);
            if (router == null)
            {
                return NotFound();
            }
            ViewData["DiapasonId"] = new SelectList(_context.Diapason, "DiapasonId", "Diapason1", router.DiapasonId);
            ViewData["PriceId"] = new SelectList(_context.Price, "PriceId", "Price1", router.PriceId);
            ViewData["SpeedId"] = new SelectList(_context.Speed, "SpeedId", "Speed1", router.SpeedId);
            return View(router);
        }

        // POST: Routers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouterId,Name,Year,PriceId,DiapasonId,SpeedId")] Router router)
        {
            if (id != router.RouterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(router);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouterExists(router.RouterId))
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
            ViewData["DiapasonId"] = new SelectList(_context.Diapason, "DiapasonId", "Diapason1", router.DiapasonId);
            ViewData["PriceId"] = new SelectList(_context.Price, "PriceId", "Price1", router.PriceId);
            ViewData["SpeedId"] = new SelectList(_context.Speed, "SpeedId", "Speed1", router.SpeedId);
            return View(router);
        }

        // GET: Routers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var router = await _context.Router
                .Include(r => r.Diapason)
                .Include(r => r.Price)
                .Include(r => r.Speed)
                .FirstOrDefaultAsync(m => m.RouterId == id);
            if (router == null)
            {
                return NotFound();
            }

            return View(router);
        }

        // POST: Routers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routSt= _context.RouterStandart.Where(d => d.RouterId== id).Include(d => d.Router).Include(d => d.Standart).ToList();
            _context.RouterStandart.RemoveRange(routSt);
            
            var router = await _context.Router.FindAsync(id);
            _context.Router.Remove(router);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouterExists(int id)
        {
            return _context.Router.Any(e => e.RouterId == id);
        }
    }
}
