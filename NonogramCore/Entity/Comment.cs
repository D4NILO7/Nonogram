using System;

namespace NonogramCore.Entity
{
    [Serializable]
    public class Comment
    {
        public int Id { get; set; }
        public string Player { get; set; }
        public string CommentContent { get; set; }
        public DateTime WroteAt { get; set; }
    }
}
