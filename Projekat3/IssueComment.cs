using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat3
{
    public class IssueComment
    {
        public string commentID { get; set; }
        public Issue Issue { get; set; }
        public string commentText { get; set; }
    public IssueComment(string id,Issue issue,string text)
    {
            this.commentID = id;
            this.Issue = issue;
            this.commentText = text;

    }
    }
}
