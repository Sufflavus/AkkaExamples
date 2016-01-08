using System;

using GithubActors.Data;


namespace GithubActors.Messages.GithubCommander
{
    public class UnableToAcceptJob
    {
        public UnableToAcceptJob(RepoKey repo)
        {
            Repo = repo;
        }


        public RepoKey Repo { get; private set; }
    }
}
