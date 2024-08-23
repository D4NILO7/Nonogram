using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NonogramCore.Entity;
using NonogramCore.Service;

namespace NonogramWeb.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IRatingsService _ratingService = new RatingServiceEF();

        [HttpGet]
        public IEnumerable<Rating> GetRatings()
        {
            return _ratingService.GetLatestRatings();
        }

        [HttpPost]
        public void PostScore(Rating rating)
        {
            rating.RatedAt = DateTime.Now;
            _ratingService.AddRating(rating);
        }
    }
}
