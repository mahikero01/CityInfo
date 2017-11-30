using CityInfo.API.Services;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            //return Ok(CitiesDatastore.Current.Cities);

            var cityEntities = _cityInfoRepository.GetCities();
            var results = new List<CityWithoutPointsOfInterestDTO>();

            foreach (var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDTO{
                    Id = cityEntity.Id,
                    Name = cityEntity.Name,
                    Description = cityEntity.Description
                });
            }
            
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            // var cityToReturn = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == id);
            
            // if (cityToReturn == null) 
            // {
            //     return NotFound();
            // }

            // return Ok(cityToReturn);

            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
               return NotFound(); 
            }

            if (includePointsOfInterest)
            {
                var cityResult = new CityDto(){
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };

                foreach (var poi in city.pointOfInterest)
                {
                    cityResult.PointsOfInterest.Add
                    (
                        new PointOfInterestDto()
                        {
                            Id = poi.Id,
                            Name = poi.Name,
                            Description = poi.Description
                        }
                    );
                }
            }
        }
    }
}