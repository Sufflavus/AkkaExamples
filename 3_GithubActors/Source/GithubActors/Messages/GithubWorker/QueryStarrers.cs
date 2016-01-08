using System;

using GithubActors.Data;


namespace GithubActors.Messages.GithubWorker
{
    public class QueryStarrers
    {
        public QueryStarrers(RepoKey key)
        {
            Key = key;
        }


        public RepoKey Key { get; private set; }
    }
}
