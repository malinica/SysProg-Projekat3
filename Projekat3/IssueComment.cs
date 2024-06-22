namespace Projekat3
{
    public class IssueComment
    {
        private string commentText;
        private double Positive;
        private double Negative;
        private double Neutral;

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
