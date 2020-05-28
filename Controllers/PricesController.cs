using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouterLab;

namespace RouterLab.Controllers
{
    public class PricesController : Controller
    {
        private readonly RouterBaseContext _context;

        public PricesController(RouterBaseContext context)
        {
            _context = context;
        }

        // GET: Prices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Price.ToListAsync());
        }
        
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {

            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {

                    using var stream = new FileStream(fileExcel.FileName, FileMode.Create);
                    await fileExcel.CopyToAsync(stream);
                    using XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled);
                    List<List<string>> Errors = new List<List<string>>();
                    foreach (IXLWorksheet worksheet in workBook.Worksheets)
                    {
                        Price price;
                        var g = (from pr in _context.Price
                                 where pr.Price1 == worksheet.Name
                                 select pr).ToList();
                        if (g.Count > 0)
                        {
                            price = g[0];
                        }
                        else
                        {
                            price = new Price
                            {
                                Price1 = worksheet.Name
                            };
                            _context.Price.Add(price);

                        }
                        await _context.SaveChangesAsync();
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Router router = new Router();
                            string name = row.Cell(1).Value.ToString();
                            if (name.Length > 50 || name.Length < 3) continue;
                            var f = (from rou in _context.Router where rou.Name == name select rou).ToList();
                            if (f.Count() > 0)
                            {
                                router = f[0];
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                router.Name = row.Cell(1).Value.ToString();
                                router.Price = price;
                                string name1 = row.Cell(2).Value.ToString();
                                if (name1.Length > 50 || name1.Length < 3) continue;
                                Speed speed;
                                var a = (from pr in _context.Speed
                                         where pr.Speed1 == row.Cell(2).Value.ToString()
                                         select pr).ToList();
                                if (a.Count() > 0)
                                {
                                    speed = a[0];
                                    router.Speed = speed;
                                }
                                else
                                {
                                    speed = new Speed
                                    {
                                        Speed1 = row.Cell(2).Value.ToString()
                                    };
                                    _context.Add(speed);
                                    router.Speed = speed;
                                }
                                string name2 = row.Cell(3).Value.ToString();
                               
                                if (name2.Length > 50 || name2.Length < 2) continue;
                                Diapason diapason;
                                var b = (from pr in _context.Diapason
                                         where pr.Diapason1 == row.Cell(3).Value.ToString()
                                         select pr).ToList();
                                if (b.Count() > 0)
                                {
                                    diapason = b[0];
                                    router.Diapason = diapason;
                                }
                                else
                                {
                                    diapason = new Diapason
                                    {
                                        Diapason1 = row.Cell(3).Value.ToString()
                                    };
                                    _context.Add(diapason);
                                    router.Diapason = diapason;
                                }
                                _context.Router.Add(router);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Routers");
        }
        public ActionResult Export(int? id)
        {
            using XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled);
            string price = _context.Price.FirstOrDefault(m => m.PriceId == id).Price1;
            var priceRouters = _context.Router.Where(f => f.PriceId == id).Include(f => f.Speed).Include(f => f.Diapason).Include(f => f.Price).ToList();
            var worksheet = workbook.Worksheets.Add(price);
            foreach (var g in priceRouters)
            {
                worksheet.Cell("A1").Value = "Назва моделі роутера";
                worksheet.Cell("B1").Value = "Рік";
                worksheet.Cell("C1").Value = "Швидкість";
                worksheet.Cell("D1").Value = "Режим роботи";
                worksheet.Row(1).Style.Font.Bold = true;
                for (int i = 0; i < priceRouters.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = priceRouters[i].Name;
                    worksheet.Cell(i + 2, 2).Value = priceRouters[i].Year;
                    worksheet.Cell(i + 2, 3).Value = priceRouters[i].Speed.Speed1;
                    worksheet.Cell(i + 2, 4).Value = priceRouters[i].Diapason.Diapason1;
                }
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Flush();

            return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"LabRouter_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }
        // GET: Prices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Price
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (price == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Routers", new { prId = id });
        }

        // GET: Prices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriceId,Price1")] Price price)
        {
            if (ModelState.IsValid)
            {
                _context.Add(price);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(price);
        }

        // GET: Prices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Price.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }
            return View(price);
        }

        // POST: Prices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriceId,Price1")] Price price)
        {
            if (id != price.PriceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(price);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceExists(price.PriceId))
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
            return View(price);
        }

        // GET: Prices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Price
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routers = _context.Router.Where(c => c.PriceId == id).Include(c => c.Price).Include(c => c.Diapason).Include(c => c.Speed).ToList();
            foreach (var c in routers)
            {
                var routSt = _context.RouterStandart.Where(d => d.RouterId == c.RouterId).Include(d => d.Router).Include(d => d.Standart).ToList();
                _context.RouterStandart.RemoveRange(routSt);
                await _context.SaveChangesAsync();
            }
            _context.Router.RemoveRange(routers);
            var price = await _context.Price.FindAsync(id);
            _context.Price.Remove(price);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceExists(int id)
        {
            return _context.Price.Any(e => e.PriceId == id);
        }
    }
}
