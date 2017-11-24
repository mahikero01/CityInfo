using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class CitiesController : Controller
    {
        [HttpGet("api/cities")]
        public JsonResult GetCities()
        {
            return new JsonResult(new List<object>()
            {
                new {id = 1, nameof="New York City"},
                new {id = 2, nameof="Antwerp"}
            });
        }
    }
}