using System;
using System.Collections.Generic;
using System.Threading;

namespace Projekat3
{
    public class Repository
    {
        private string owner;
        private string name;
        public DateTime CreatedOnTime { get; set; }
        private List<Issue> issueList { get; set; }
        private ReaderWriterLockSlim _lock { get; set; }
        public Repository(string o, string n)
        {
            this.owner = o;
            this.name = n;
            CreatedOnTime = DateTime.Now;
            issueList = new List<Issue>();
            _lock = new ReaderWriterLockSlim();
        }
        public void AddIssue(Issue i)
        {
            _lock.EnterWriteLock();
            try
            {
                issueList.Add(i);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _lock.ExitWriteLock();
            }

        }
        public override string ToString()
        {
            _lock.EnterReadLock();
            try
            {
                string text = "";
                if (issueList.Count > 0)
                {

                    foreach (var i in issueList)
                    {
                        text += i.ToString();
                    }
                }
                else
                    text += "No issues \n";
                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
