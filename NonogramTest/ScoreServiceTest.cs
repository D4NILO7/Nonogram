using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramCore.Entity;
using NonogramCore.Service;

namespace NonogramTest
{
    [TestClass]
    public class ScoreServiceTest
    {
        [TestMethod]
        public void AddTest1()
        {
            var service = CreateService();

            service.AddScore(new Score { Player = "Eric", Points = 200, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Kyle", Points = 300, PlayedAt = DateTime.Now });
            Assert.AreEqual(2, service.GetTopScores().Count);

            service.AddScore(new Score { Player = "Stan", Points = 400, PlayedAt = DateTime.Now });
            Assert.AreEqual(3, service.GetTopScores().Count);

            Assert.AreEqual("Stan", service.GetTopScores()[0].Player);
            Assert.AreEqual(400, service.GetTopScores()[0].Points);
            Assert.AreEqual("Eric", service.GetTopScores()[2].Player);
            Assert.AreEqual(200, service.GetTopScores()[2].Points);
        }


        [TestMethod]
        public void AddTest2()
        {
            var service = CreateService();

            service.AddScore(new Score { Player = "Kenny", Points = 500, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Kyle", Points = 600, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Stan", Points = -6, PlayedAt = DateTime.Now });

            Assert.AreEqual(3, service.GetTopScores().Count);

            Assert.AreEqual("Kyle", service.GetTopScores()[0].Player);
            Assert.AreEqual(600, service.GetTopScores()[0].Points);

            Assert.AreEqual("Kenny", service.GetTopScores()[1].Player);
            Assert.AreEqual(500, service.GetTopScores()[1].Points);

            Assert.AreEqual("Stan", service.GetTopScores()[2].Player);
            Assert.AreEqual(-6, service.GetTopScores()[2].Points);
        }

        [TestMethod]
        public void AddTest3()
        {
            var service = CreateService();

            service.AddScore(new Score { Player = "Kenny", Points = 500, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Kyle", Points = 600, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Eric", Points = 700, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Stan", Points = 400, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Butters", Points = 300, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Timmy", Points = 200, PlayedAt = DateTime.Now });

            Assert.AreEqual(5, service.GetTopScores().Count);

            Assert.AreEqual("Eric", service.GetTopScores()[0].Player);
            Assert.AreEqual(700, service.GetTopScores()[0].Points);

            Assert.AreEqual("Kyle", service.GetTopScores()[1].Player);
            Assert.AreEqual(600, service.GetTopScores()[1].Points);

            Assert.AreEqual("Kenny", service.GetTopScores()[2].Player);
            Assert.AreEqual(500, service.GetTopScores()[2].Points);

            Assert.AreEqual("Stan", service.GetTopScores()[3].Player);
            Assert.AreEqual(400, service.GetTopScores()[3].Points);

            Assert.AreEqual("Butters", service.GetTopScores()[4].Player);
            Assert.AreEqual(300, service.GetTopScores()[4].Points);
        }

        [TestMethod]
        public void ResetTest()
        {
            var service = CreateService();

            service.AddScore(new Score { Player = "Eric", Points = 500, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Timmy", Points = 400, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Jimmy", Points = 300, PlayedAt = DateTime.Now });
            service.AddScore(new Score { Player = "Kenny", Points = 200, PlayedAt = DateTime.Now });

            service.ResetScore();
            Assert.AreEqual(0, service.GetTopScores().Count);
        }


        private IScoreService CreateService()
        {
            var service = new ScoreServiceEF();
            service.ResetScore();
            return service;
        }
    }
}