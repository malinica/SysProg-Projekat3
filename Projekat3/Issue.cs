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
        private ReaderWriterLockSlim _lock;

        public Issue(string iID, string iText, string iC)
        {
            _lock = new ReaderWriterLockSlim();
            _comments = new List<IssueComment>();
            this.issueID = iID;
            this.issueText = iText;
            this.issueCreator = iC;
        }
        public Issue()
        {
            _lock = new ReaderWriterLockSlim();
            _comments = new List<IssueComment>();

        }
        public void AddComment(IssueComment iComm)
        {
            _lock.EnterWriteLock();
            try
            {
                _comments.Add(iComm);
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

        public void AddComments(List<IssueComment> iCommList)
        {
            _lock.EnterWriteLock();
            try
            {
                foreach(var iComm in iCommList)
                _comments.Add(iComm);
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
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
