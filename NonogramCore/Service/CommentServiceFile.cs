using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    [Serializable]
    public class CommentServiceFile : ICommentsService
    {
        private const string FileName = "comments.bin";
        private List<Comment> _comments = new List<Comment>();

        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
            SaveComments();
        }

        public IList<Comment> GetLatestComments()
        {
            LoadComments();
            return _comments.OrderByDescending(c => c.WroteAt).Take(5).ToList();
        }


        public void DeleteComments()
        {
            _comments.Clear();
            File.Delete(FileName);
        }

        private void SaveComments()
        {
            using (var fs = File.OpenWrite(FileName))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, _comments);
            }
        }

        private void LoadComments()
        {
            if (File.Exists(FileName))
            {
                using (var fs = File.OpenRead(FileName))
                {
                    var bf = new BinaryFormatter();
                    _comments = (List<Comment>) bf.Deserialize(fs);
                }
            }
        }
    }
}