using System;

using GithubActors.Data;


namespace GithubActors.Messages.GithubCommander
{
    public class CanAcceptJob
    {
        public CanAcceptJob(RepoKey repo)
        {
            Repo = repo;
        }


        public RepoKey Repo { get; private set; }
    }
}
