using System;

using GithubActors.Data;


namespace GithubActors.Messages.GithubCoordinator
{
    /// <summary>
    /// Let the subscribers know we failed
    /// </summary>
    public class JobFailed
    {
        public JobFailed(RepoKey repo)
        {
            Repo = repo;
        }


        public RepoKey Repo { get; private set; }
    }
}
