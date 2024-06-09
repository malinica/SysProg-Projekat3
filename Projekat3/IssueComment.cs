using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat3
{
    public class IssueComment
    {
        public string issueID { get; set; }
        public string issueOwner  { get; set; }
        public string issueText { get; set; }
        public string commentText { get; set; }
        public double Positive { get; set; }
        public double Negative { get; set; }
        public double Neutral { get; set; }
        public IssueComment(string iID,string iOwner, string iText,string cText, double positive,double neutral, double negative)
    {
            this.issueID = iID;
            this.issueOwner = iOwner;
            this.issueText = iText;
            this.commentText = cText;
            this.Positive = positive;
            this.Negative = negative;
            this.Neutral = neutral;
    
        }
        public override string ToString()
        {
            return $"IssueID: {issueID}, IssueOwner: {issueOwner}, IssueText: {issueText}, CommentText: {commentText}, Positive: {Positive}, Neutral: {Neutral}, Negative: {Negative}";
        }
        public string GetCommentDetails()
        {
            return $"Comment Text: {commentText}\nPositive: {Positive}\nNeutral: {Neutral}\nNegative: {Negative}";
        }
    }
}
