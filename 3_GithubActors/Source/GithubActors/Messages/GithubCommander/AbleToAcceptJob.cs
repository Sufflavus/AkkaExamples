using System;

using GithubActors.Data;


namespace GithubActors.Messages.GithubCommander
{
    public class AbleToAcceptJob
    {
        public AbleToAcceptJob(RepoKey repo)
        {
            Repo = repo;
        }


        public RepoKey Repo { get; private set; }
    }
}
