using System;


namespace GithubActors.Data
{
    public class RepoKey
    {
        public RepoKey(string owner, string repo)
        {
            Repo = repo;
            Owner = owner;
        }


        public string Owner { get; private set; }

        public string Repo { get; private set; }
    }
}
