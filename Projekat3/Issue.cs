using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Projekat3
{
    public class Issue
    {
        private string issueCreator;
        private string issueID;
        private string issueText;
        private List<IssueComment> _comments;

        public Issue(string iID, string iText, string iC)
        {
            _comments = new List<IssueComment>();
            this.issueID = iID;
            this.issueText = iText;
            this.issueCreator = iC;
        }

        public Issue()
        {
            _comments = new List<IssueComment>();

        }

        public void AddComment(IssueComment iComm)
        {
            try
            {
                _comments.Add(iComm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddComments(List<IssueComment> iCommList)
        {
            try
            {
                foreach(var iComm in iCommList)
                _comments.Add(iComm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override string ToString()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine( $"IssueID: {issueID}, Issue Creator: {issueCreator} , IssueText: {issueText} \n");
                if (_comments.Count > 0)
                {
                    foreach (var c in _comments)
                    {
                        sb.AppendLine(c.GetCommentDetails());
                    }
                }
                else
                {
                    sb.AppendLine(" No comments \n");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error in Issue .ToString()";
            }
        }
    }
}
