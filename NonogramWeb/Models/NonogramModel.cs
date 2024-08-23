using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NonogramCore.Core;
using NonogramCore.Entity;
using NonogramCore.Service;

namespace NonogramWeb.Models
{
    public class NonogramModel
    {
        public Field Field { get; set; }
        public IList<Score> Scores { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<Rating> Ratings { get; set;}
    }
}
