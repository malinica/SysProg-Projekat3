using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Projekat3
{
    public class Repository
    {
        private string owner;
        private string name;
        public DateTime CreatedOnTime;
        private List<Issue> issueList;
        public Repository(string o, string n)
        {
            this.owner = o;
            this.name = n;
            CreatedOnTime = DateTime.Now;
            issueList = new List<Issue>();
        }
        public void AddIssue(Issue i)
        {
            try
            {
                issueList.Add(i);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void AddIssues(List<Issue> iList)
        {
            try
            {
                foreach(var i in iList)
                issueList.Add(i);
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
                if (issueList.Count > 0)
                {

                    foreach (var i in issueList)
                    {
                        sb.AppendLine(i.ToString());
                    }
                }
                else
                    sb.AppendLine("No issues \n");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error in Repository .ToString()";
            }
        }
    }
}
