using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name="GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);

            if (pointOfInterest == null) 
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,
                [FromBody] PointOfInterestForCreationDto pointOfInterest )
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name) 
            {
                ModelState.AddModelError("Description", "The provided description should be different from name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound();
            }

            var maxPointOfInteresId = CitiesDatastore.Current.Cities.SelectMany(    
                    c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest =  new PointOfInterestDto()
            {
                Id = ++maxPointOfInteresId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", 
                    new {cityId = cityId, id = finalPointOfInterest.Id}, finalPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, 
                [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name) 
            {
                ModelState.AddModelError("Description", "The provided description should be different from name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);

            if (pointOfInterestFromStore == null) 
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }
    }
}