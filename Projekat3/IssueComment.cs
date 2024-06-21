namespace Projekat3
{
    public class IssueComment
    {
        public string commentText { get; set; }
        public double Positive { get; set; }
        public double Negative { get; set; }
        public double Neutral { get; set; }

        public IssueComment(string cText, double positive, double neutral, double negative)
        {

            this.commentText = cText;
            this.Positive = positive;
            this.Negative = negative;
            this.Neutral = neutral;

        }
        public string GetCommentDetails()
        {
            return $"Comment Text: {commentText}\nPositive: {Positive}\nNeutral: {Neutral}\nNegative: {Negative}";
        }
    }
}
