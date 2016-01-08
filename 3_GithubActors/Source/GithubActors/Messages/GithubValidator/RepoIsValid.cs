using System;


namespace GithubActors.Messages.GithubValidator
{
    /// <summary>
    /// This is a valid repository
    /// </summary>
    public class RepoIsValid
    {
        /*
         * Using singleton pattern here since it's a stateless message.
         * 
         * Considered to be a good practice to eliminate unnecessary garbage collection,
         * and it's used internally inside Akka.NET for similar scenarios.
         */

        private static readonly RepoIsValid _instance = new RepoIsValid();


        private RepoIsValid()
        {
        }


        public static RepoIsValid Instance
        {
            get { return _instance; }
        }
    }
}
