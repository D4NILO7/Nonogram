using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    public class RatingServiceFile : IRatingsService
    {
        private const string FileName = "rating.bin";
        private List<Rating> _ratings = new List<Rating>();
        public void AddRating(Rating rating)
        {
            _ratings.Add(rating);
            SaveRatings();
        }

        public IList<Rating> GetLatestRatings()
        {
            LoadRatings();
            return _ratings.OrderByDescending(s => s.RatedAt).Take(5).ToList();
        }

        public void DeleteRatings()
        {
            _ratings.Clear();
            File.Delete(FileName);
        }

        private void SaveRatings()
        {
            using (var fs = File.OpenWrite(FileName))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, _ratings);
            }
        }

        private void LoadRatings()
        {
            if (File.Exists(FileName))
            {
                using (var fs = File.OpenRead(FileName))
                {
                    var bf = new BinaryFormatter();
                    _ratings = (List<Rating>)bf.Deserialize(fs);
                }
            }
        }

    }
}
