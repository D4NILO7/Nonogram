using System;

namespace NonogramCore.Entity
{
    [Serializable]
    public class Rating
    {
        public int Id { get; set; }
        public string Player { get; set; }
        public int RatingValue { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
