using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet()]
        public IActionResult GetCities()
        {
            return Ok(CitiesDatastore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cityToReturn = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (cityToReturn == null) 
            {
                return NotFound();
            }

            return Ok(cityToReturn);
        }
    }
}