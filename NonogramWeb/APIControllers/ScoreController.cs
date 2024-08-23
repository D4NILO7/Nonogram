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
    //https://localhost:44345/api/Score
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private IScoreService _scoreService = new ScoreServiceEF();

        [HttpGet]
        public IEnumerable<Score> GetScores()
        {
            return _scoreService.GetTopScores();
        }

        [HttpPost]
        public void PostScore(Score score)
        {
            score.PlayedAt = DateTime.Now;
            _scoreService.AddScore(score);
        }

    }
}
