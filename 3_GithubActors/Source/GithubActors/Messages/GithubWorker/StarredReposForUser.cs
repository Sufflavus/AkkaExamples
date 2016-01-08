using System;
using System.Collections.Generic;

using Octokit;


namespace GithubActors.Messages.GithubWorker
{
    public class StarredReposForUser
    {
        public StarredReposForUser(string login, IEnumerable<Repository> repos)
        {
            Repos = repos;
            Login = login;
        }


        public string Login { get; private set; }

        public IEnumerable<Repository> Repos { get; private set; }
    }
}
