using System;


namespace GithubActors.Messages.GithubValidator
{
    public class ValidateRepo
    {
        public ValidateRepo(string repoUri)
        {
            RepoUri = repoUri;
        }


        public string RepoUri { get; private set; }
    }
}
