using System;


namespace GithubActors.Messages.GithubValidator
{
    public class InvalidRepo
    {
        public InvalidRepo(string repoUri, string reason)
        {
            Reason = reason;
            RepoUri = repoUri;
        }


        public string Reason { get; private set; }
        public string RepoUri { get; private set; }
    }
}
