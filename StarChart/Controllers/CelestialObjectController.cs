using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{Id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {

            CelestialObject obj = _context.CelestialObjects.Where(o => o.Id == id).FirstOrDefault();
            

            if (obj.Id != id)
            {
                return NotFound();
            }

            //List<CelestialObject> sat=_context.CelestialObjects.Where(o => o.Id == obj.OrbitedObjectId).ToList<>;

            return Ok();
        }

        [HttpGet ("{name}")]
        public IActionResult GetByName(string name)
        {
            CelestialObject obj = _context.CelestialObjects.Where(o => o.Name == name).FirstOrDefault();

            if (obj.Name != name)
            {
                return NotFound();
            }

            return Ok();


        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _context.CelestialObjects.Find();

            return Ok();
        }


    }
}
