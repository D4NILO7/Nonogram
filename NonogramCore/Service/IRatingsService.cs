using System.Collections.Generic;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    public interface IRatingsService
    {
        void AddRating(Rating rating);
        IList<Rating> GetLatestRatings();
        void DeleteRatings();
    }
}
