using System;
using System.Collections.Generic;
using System.Threading;

namespace Projekat3
{
    public class Issue
    {
        private string issueCreator { get; set; }
        private string issueID { get; set; }
        private string issueText { get; set; }
        private List<IssueComment> _comments { get; set; }
        private ReaderWriterLockSlim _lock { get; set; }

        public Issue(string iID, string iText,string iC)
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

        public override string ToString()
        {
            _lock.EnterReadLock();
            try
            {
                string text = "";
                text += $"IssueID: {issueID}, Issue Creator: {issueCreator} , IssueText: {issueText} \n";
                if (_comments.Count > 0)
                {
                foreach (var c in _comments)
                {
                    text += c.GetCommentDetails();
                }
                }
                        else
                {
                    text += " No comments \n";
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
