using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using NonogramCore.Core;
using NonogramCore.Entity;
using NonogramCore.Service;
using NonogramWeb.Models;


namespace NonogramWeb.Controllers
{
    public class NonogramController : Controller
    {
        private const string FieldSessionKey = "field";
        private const string PlayerSessionKey = "playerkey";
        private IScoreService _scoreService = new ScoreServiceEF();
        private ICommentsService _commentService = new CommentServiceEF();
        private IRatingsService _ratingService = new RatingServiceEF();
        public IActionResult Index()
        {
            var field = new Field(Difficulty.Medium);
            HttpContext.Session.SetObject(FieldSessionKey, field);

            var scores = _scoreService.GetTopScores();
            var comments = _commentService.GetLatestComments();
            var ratings = _ratingService.GetLatestRatings();
            var model = new NonogramModel {Field = field, Scores = scores, Comments = comments, Ratings = ratings};
             
        return View("Index", CreateModel());
        }

        public IActionResult ActionOnTile(int row, int column)
        {
            var field = (Field)HttpContext.Session.GetObject(FieldSessionKey);
            if (!field.IsSolved())
            {
                if (field.ActionState == ActionState.Color)
                {
                    field.ColorTile(row, column);
                }
                else
                {
                    field.ExcludeTile(row, column);
                }

                HttpContext.Session.SetObject(FieldSessionKey, field);
                field = (Field) HttpContext.Session.GetObject(FieldSessionKey);
                string player = HttpContext.Session.GetString(PlayerSessionKey);
                if (player == null)
                {
                    player = "player";
                }
                if (field.IsSolved())
                {
                    _scoreService.AddScore(new Score()
                        {PlayedAt = DateTime.Now, Player = player, Points = field.GetScore()});
                }
            }

            return View("Index", CreateModel());
        }

        public IActionResult ColorTile()
        {
            var field = (Field)HttpContext.Session.GetObject(FieldSessionKey);
            field.ActionState = ActionState.Color;
            HttpContext.Session.SetObject(FieldSessionKey, field);
            return View("Index", CreateModel());
        }

        public IActionResult ExcludeTile()
        {
            var field = (Field)HttpContext.Session.GetObject(FieldSessionKey);
            field.ActionState = ActionState.Exclude;
            HttpContext.Session.SetObject(FieldSessionKey, field);
            return View("Index", CreateModel());
        }

        private NonogramModel CreateModel()
        {
            var field = (Field) HttpContext.Session.GetObject(FieldSessionKey);
            var scores = _scoreService.GetTopScores();
            var comments = _commentService.GetLatestComments();
            var ratings = _ratingService.GetLatestRatings();

            return new NonogramModel {Field = field, Scores = scores, Comments = comments, Ratings = ratings};
        }

        public IActionResult AddRating(int rating)
        {

            string player = HttpContext.Session.GetString(PlayerSessionKey);
            if (player == null)
            {
                player = "player";
            }
            _ratingService.AddRating(new Rating() {Player = player, RatedAt = DateTime.Now, RatingValue = rating});
            // _ratingService.AddRating(rating);
            //rating.RatedAt = DateTime.Now;
            //rating.Player = "Jozef";

            return View("Index", CreateModel());
        }

        public IActionResult AddComment(string comment)
        {
            string player = HttpContext.Session.GetString(PlayerSessionKey);
            if (player == null)
            {
                player = "player";
            }
            _commentService.AddComment(new Comment(){CommentContent = comment, Player = player, WroteAt = DateTime.Now});
           /* comment.WroteAt = DateTime.Now;
            comment.Player = "Jozef";
            _commentService.AddComment(comment);*/

            return View("Index", CreateModel());
        }

    }
}
