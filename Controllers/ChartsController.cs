using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouterLab;

namespace CarsLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly RouterBaseContext _context;
        public ChartsController(RouterBaseContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var bodyCar = _context.Diapason.Include(c => c.Router).ToList();
            List<object> carBody = new List<object>();
            carBody.Add(new[] { "Режим роботи роутера", "Кількість роутерів" });
            foreach (var c in bodyCar)
            {
                carBody.Add(new object[] { c.Diapason1, c.Router.Count() });
            }
            return new JsonResult(carBody);
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var engines = _context.Speed.Include(c => c.Router).ToList();
            List<object> carEng = new List<object>();
            carEng.Add(new[] { "Швидкість", "Кількість роутерів" });
            foreach (var c in engines)
            {
                carEng.Add(new object[] { c.Speed1, c.Router.Count() });
            }
            return new JsonResult(carEng);
        }
    }
}