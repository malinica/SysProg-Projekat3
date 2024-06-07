using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat3
{
    public class IssueReview
    {
        public IssueComment IssueComment{get;set;}
        public double Positive { get; set; }
        public double Negative { get; set; }
        public double Neutral { get; set; }
        public IssueReview(IssueComment issueCom,double positive,double negative,double neutral)
        {
            this.IssueComment = issueCom;
            this.Positive = positive;
            this.Negative = negative;
            this.Neutral = neutral;
        }
    }
}
