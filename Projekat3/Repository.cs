using System;
using System.Collections.Generic;
using System.Threading;

namespace Projekat3
{
    public class Repository
    {
        public string owner;
        public DateTime CreatedOnTime { get; set; }
        public List<Issue> issueList { get; set; }
        private ReaderWriterLockSlim _lock { get; set; }
        public Repository()
        {
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
                text = "Owner " + owner + "\n";
                foreach (var i in issueList)
                {
                    text += i.ToString();
                }
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
