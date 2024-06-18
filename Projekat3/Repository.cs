using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekat3
{
    public class Repository
    {
        public string owner;
        public DateTime CreatedOnTime { get; set; }
        public List<Issue> issueList { get; set; }
        private ReaderWriterLockSlim _lock;
        public Repository()
        {
            CreatedOnTime = DateTime.Now;
            issueList = new List<Issue>();
            _lock = new ReaderWriterLockSlim();
        }
        public void AddIssue(Issue i)
        {
            try
            {
                _lock.EnterWriteLock();
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
            try
            {
                string text = "";

                _lock.EnterReadLock();
                text= "Owner " + owner+ "\n";
                foreach (var i in issueList)
                {
                    text += i.ToString();
                }
                return text;
            }
            catch(Exception ex)
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
