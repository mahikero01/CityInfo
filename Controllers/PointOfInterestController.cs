using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;

        public PointOfInterestController (ILogger<PointOfInterestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try 
            {
                throw new Exception("Exception Problem");

                var city = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null) 
                {
                    _logger.LogInformation($"city with ID {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            } catch(Exception ex)
            {
                _logger.LogCritical($"Exceptions while getting points of interest for city with ID {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
            
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

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
                [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
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

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name) 
            {
                ModelState.AddModelError("Description", "The provided description should be different from name.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
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

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }
    }
}