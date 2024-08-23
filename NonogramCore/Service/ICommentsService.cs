using System.Collections.Generic;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    public interface ICommentsService
    {
        void AddComment(Comment comment);
        IList<Comment> GetLatestComments(); 
        void DeleteComments();
    }
}
