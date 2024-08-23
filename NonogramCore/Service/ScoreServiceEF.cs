using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    public class ScoreServiceEF : IScoreService
    {
        public void AddScore(Score score)
        {
            using (var context = new NonogramDbContext())
            {
                context.Scores.Add(score);
                context.SaveChanges();
            }
        }

        public IList<Score> GetTopScores()
        {
            using (var context = new NonogramDbContext())
            {
                return (from s in context.Scores orderby s.Points descending select s).Take(5).ToList();
            }

        }

        public void ResetScore()
        {
            using (var context = new NonogramDbContext())
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Scores");
            }
        }
    }
}
