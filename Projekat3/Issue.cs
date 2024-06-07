using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat3
{
    public class Issue
    {
        string issueID { get; set; }
        public string owner { get; set; }
        public string title { get; set; }
        public Issue(string i,string o,string t)
        {
           this.issueID = i;
            this.owner = o;
            this.title = t;
        }
    }
}
