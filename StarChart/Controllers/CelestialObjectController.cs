using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("{id:int}",Name="GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestialObject = null;
            celestialObject = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);
            if (celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(co => co.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();
            foreach (var co in celestialObjects)
            {
                co.Satellites = _context.CelestialObjects.Where(cos => cos.OrbitedObjectId == co.Id).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();
            foreach (var co in celestialObjects)
            {
                co.Satellites = _context.CelestialObjects.Where(cos => cos.OrbitedObjectId == co.Id).ToList();
            }

            return Ok(celestialObjects);
        }






    }
}
