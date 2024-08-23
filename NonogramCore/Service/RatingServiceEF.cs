using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    [Serializable]
    public class RatingServiceEF : IRatingsService
    {
        public void AddRating(Rating rating)
        {
            using (var context = new NonogramDbContext())
            {
                context.Ratings.Add(rating);
                context.SaveChanges();
            }
        }

        public IList<Rating> GetLatestRatings()
        {
            using (var context = new NonogramDbContext())
            {
                return (from s in context.Ratings orderby s.RatedAt descending select s).Take(5).ToList();
            }
        }

        public void DeleteRatings()
        {
            using (var context = new NonogramDbContext())
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Ratings");
            }
        }
    }
}
