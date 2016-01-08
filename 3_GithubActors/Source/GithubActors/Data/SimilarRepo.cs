using System;

using Octokit;


namespace GithubActors.Data
{
    /// <summary>
    /// used to sort the list of similar repos
    /// </summary>
    public class SimilarRepo : IComparable<SimilarRepo>
    {
        public SimilarRepo(Repository repo)
        {
            Repo = repo;
        }


        public Repository Repo { get; private set; }

        public int SharedStarrers { get; set; }


        public int CompareTo(SimilarRepo other)
        {
            return SharedStarrers.CompareTo(other.SharedStarrers);
        }
    }
}
