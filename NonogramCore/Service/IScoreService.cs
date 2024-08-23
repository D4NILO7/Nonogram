using System.Collections.Generic;
using NonogramCore.Entity;

namespace NonogramCore.Service
{
    public interface IScoreService
    {
        void AddScore(Score score);
        IList<Score> GetTopScores();
        void ResetScore();
    }
}
