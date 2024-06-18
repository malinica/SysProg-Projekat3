using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekat3
{
    public class Issue
    {
        public string issueID { get; set; }
        public string issueText { get; set; }
        public List<IssueComment> _comments { get; set; }
        private ReaderWriterLockSlim _lock { get; set; }

        public Issue(string iID, string iOwner, string iText)
        {
            _lock = new ReaderWriterLockSlim();
            _comments = new List<IssueComment>();
            this.issueID = iID;
            this.issueText = iText;
        }
        public Issue()
        {
            _lock = new ReaderWriterLockSlim();
            _comments = new List<IssueComment>();

        }
        public void AddComment(IssueComment iComm)
        {
            try
            {
                _lock.EnterWriteLock();
                _comments.Add(iComm);
            }
            catch(Exception ex)
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

                _lock.EnterReadLock();
                string text = "";
                text += $"IssueID: {issueID}, IssueText: {issueText}";
                foreach (var c in _comments)
                {
                    text += c.GetCommentDetails();
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
