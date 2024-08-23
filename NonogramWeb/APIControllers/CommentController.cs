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
    public class CommentController : ControllerBase
    {
        private ICommentsService _commentService = new CommentServiceEF();

        [HttpGet]
        public IEnumerable<Comment> GetComments()
        {
            return _commentService.GetLatestComments();
        }

        [HttpPost]
        public void PostScore(Comment comment)
        {
            comment.WroteAt = DateTime.Now;
            _commentService.AddComment(comment);
        }
    }
}
