using System;


namespace GithubActors.Messages.GithubWorker
{
    /// <summary>
    /// Query an individual starrer
    /// </summary>
    public class QueryStarrer
    {
        public QueryStarrer(string login)
        {
            Login = login;
        }


        public string Login { get; private set; }
    }
}
