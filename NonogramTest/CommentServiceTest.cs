using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramCore.Entity;
using NonogramCore.Service;

namespace NonogramTest
{
    [TestClass]
    public class CommentServiceTest
    {
        [TestMethod]
        public void AddTest1()
        {
            var service = CreateService();

            service.AddComment(new Comment { Player = "Sergio", CommentContent = "Awsome", WroteAt = DateTime.Now });
            service.AddComment(new Comment { Player = "Thibout", CommentContent = "Not bad", WroteAt = DateTime.Now });

            Assert.AreEqual(2, service.GetLatestComments().Count);

            service.AddComment(new Comment { Player = "Raphael", CommentContent = "Best game ever", WroteAt = DateTime.Now });

            Assert.AreEqual(3, service.GetLatestComments().Count);

        }

        [TestMethod]
        public void AddTest2()
        {
            var service = CreateService();

            service.AddComment(new Comment { Player = "Sergio", CommentContent = "Awsome", WroteAt = DateTime.Now });
            service.AddComment(new Comment { Player = "Thibout", CommentContent = "Not bad", WroteAt = DateTime.Now });
            service.AddComment(new Comment { Player = "Raphael", CommentContent = "Best game ever", WroteAt = DateTime.Now });
            service.AddComment(new Comment { Player = "Lucas", CommentContent = "Worst", WroteAt = DateTime.Now });
            service.AddComment(new Comment { Player = "Marcelo", CommentContent = "Doable", WroteAt = DateTime.Now });
            service.AddComment(new Comment { Player = "Toni", CommentContent = "Too ez", WroteAt = DateTime.Now });

            Assert.AreEqual(5, service.GetLatestComments().Count);

            Assert.AreEqual("Toni", service.GetLatestComments()[0].Player);
            Assert.AreEqual("Marcelo", service.GetLatestComments()[1].Player);
            Assert.AreEqual("Lucas", service.GetLatestComments()[2].Player);
            Assert.AreEqual("Raphael", service.GetLatestComments()[3].Player);
            Assert.AreEqual("Thibout", service.GetLatestComments()[4].Player);

            Assert.AreEqual("Too ez", service.GetLatestComments()[0].CommentContent);
            Assert.AreEqual("Doable", service.GetLatestComments()[1].CommentContent);
            Assert.AreEqual("Worst", service.GetLatestComments()[2].CommentContent);
            Assert.AreEqual("Best game ever", service.GetLatestComments()[3].CommentContent);
            Assert.AreEqual("Not bad", service.GetLatestComments()[4].CommentContent);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var service = CreateService();
            service.AddComment(new Comment { Player = "Sergio", CommentContent = "Awsome", WroteAt = DateTime.Now });
            service.AddComment(new Comment { Player = "Thibout", CommentContent = "Not bad", WroteAt = DateTime.Now });
            
            Assert.AreEqual(2, service.GetLatestComments().Count);
            service.DeleteComments();

            Assert.AreEqual(0, service.GetLatestComments().Count);

        }

        private ICommentsService CreateService()
        {
            var service = new CommentServiceEF();
            service.DeleteComments();
            return service;
        }
    }
}
