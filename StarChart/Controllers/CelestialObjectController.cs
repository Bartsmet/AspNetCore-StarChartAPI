using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public CelestialObjectController(ApplicationDbContext context)

        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {

            CelestialObject obj = _context.CelestialObjects.Find(id);


            if (obj == null)
            {
                return NotFound();
            }

            obj.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id).ToList();

            return Ok(obj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(o => o.Name == name).ToList();

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);

        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var _celestialObject =_context.CelestialObjects.Find(id);

            if (_celestialObject == null)
            {
                return NotFound();
            }

            _celestialObject.Name = celestialObject.Name;
            _celestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            _celestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.Update(_celestialObject);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var _celestialObject = _context.CelestialObjects.Find(id);
            if (_celestialObject == null)
            {
                return NotFound();
            }

            _celestialObject.Name = name;
            _context.Update(_celestialObject);
            _context.SaveChanges();

            return NoContent();


        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var CelestialObjects = _context.CelestialObjects.Where(o => (o.Id == id) || o.OrbitedObjectId == id).ToList();

            if (!CelestialObjects.Any())
            {
                return NotFound();
            }

            _context.RemoveRange(CelestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
