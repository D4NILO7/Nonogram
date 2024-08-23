using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramCore.Entity;
using NonogramCore.Service;

namespace NonogramTest
{
    [TestClass]
    public class RatingServiceTest
    {
        [TestMethod]
        public void AddTest1()
        {
            var service = CreateService();

            service.AddRating(new Rating {Player = "Eric", RatingValue = 4, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Kyle", RatingValue = 3, RatedAt = DateTime.Now});
            Assert.AreEqual(2, service.GetLatestRatings().Count);

            service.AddRating(new Rating {Player = "Stan", RatingValue = 2, RatedAt = DateTime.Now});
            Assert.AreEqual(3, service.GetLatestRatings().Count);

            Assert.AreEqual("Stan", service.GetLatestRatings()[0].Player);
            Assert.AreEqual("Kyle", service.GetLatestRatings()[1].Player);
            Assert.AreEqual("Eric", service.GetLatestRatings()[2].Player);
        }

        [TestMethod]
        public void AddTest2()
        {
            var service = CreateService();

            service.AddRating(new Rating {Player = "Eric", RatingValue = 5, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Kyle", RatingValue = 4, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Stan", RatingValue = 3, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Kenny", RatingValue = 2, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Randy", RatingValue = 1, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Vandy", RatingValue = 0, RatedAt = DateTime.Now});

            Assert.AreEqual(5, service.GetLatestRatings().Count);

            Assert.AreEqual("Vandy", service.GetLatestRatings()[0].Player);
            Assert.AreEqual("Randy", service.GetLatestRatings()[1].Player);
            Assert.AreEqual("Kenny", service.GetLatestRatings()[2].Player);
            Assert.AreEqual("Stan", service.GetLatestRatings()[3].Player);
            Assert.AreEqual("Kyle", service.GetLatestRatings()[4].Player);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var service = CreateService();
            service.AddRating(new Rating {Player = "Eric", RatingValue = 5, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Kyle", RatingValue = 4, RatedAt = DateTime.Now});
            service.AddRating(new Rating {Player = "Stan", RatingValue = 3, RatedAt = DateTime.Now});

            Assert.AreEqual(3, service.GetLatestRatings().Count);

            service.DeleteRatings();
            Assert.AreEqual(0, service.GetLatestRatings().Count);
        }

        private IRatingsService CreateService()
        {
            var service = new RatingServiceEF();
            service.DeleteRatings();
            return service;
        }
    }
}