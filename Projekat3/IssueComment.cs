namespace Projekat3
{
    public class IssueComment
    {
        private string commentText { get; set; }
        private double Positive { get; set; }
        private double Negative { get; set; }
        private double Neutral { get; set; }

        public IssueComment(string cText, double positive, double neutral, double negative)
        {

            this.commentText = cText;
            this.Positive = positive;
            this.Negative = negative;
            this.Neutral = neutral;

        }
        public string GetCommentDetails()
        {
            return $"Comment Text: {commentText}\nPositive: {Positive}\nNeutral: {Neutral}\nNegative: {Negative} \n";
        }
    }
}
