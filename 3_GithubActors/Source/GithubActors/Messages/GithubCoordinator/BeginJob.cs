using System;

using GithubActors.Data;


namespace GithubActors.Messages.GithubCoordinator
{
    public class BeginJob
    {
        public BeginJob(RepoKey repo)
        {
            Repo = repo;
        }


        public RepoKey Repo { get; private set; }
    }
}
