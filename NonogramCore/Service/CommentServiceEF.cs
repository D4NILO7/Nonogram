using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    public class CommentServiceEF : ICommentsService
    {
        public void AddComment(Comment comment)
        {
            using (var context = new NonogramDbContext())
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }
        }

        public IList<Comment> GetLatestComments()
        {
            using (var context = new NonogramDbContext())
            {
                return (from c in context.Comments orderby c.WroteAt descending select c).Take(5).ToList();
            }
        }

        public void DeleteComments()
        {
            using (var context = new NonogramDbContext())
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Comments");
            }
        }
    }
}
