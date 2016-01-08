using System;
using System.Linq;

using Akka.Actor;

using GithubActors.Data;
using GithubActors.Messages.GithubRepositoryAnalysis;
using GithubActors.Messages.GithubWorker;

using Octokit;


namespace GithubActors.Actors
{
    /// <summary>
    /// Individual actor responsible for querying the Github API
    /// </summary>
    public class GithubWorkerActor : ReceiveActor
    {
        private readonly Func<IGitHubClient> _gitHubClientFactory;
        private IGitHubClient _gitHubClient;


        public GithubWorkerActor(Func<IGitHubClient> gitHubClientFactory)
        {
            _gitHubClientFactory = gitHubClientFactory;
            InitialReceives();
        }


        protected override void PreStart()
        {
            _gitHubClient = _gitHubClientFactory();
        }


        private void InitialReceives()
        {
            //query an individual starrer
            Receive<RetryableQuery>(query => query.Query is QueryStarrer, query =>
            {
                // ReSharper disable once PossibleNullReferenceException (we know from the previous IS statement that this is not null)
                string starrer = (query.Query as QueryStarrer).Login;

                // close over the Sender in an instance variable
                IActorRef sender = Sender;
                _gitHubClient.Activity.Starring.GetAllForUser(starrer)
                    .ContinueWith<object>(tr =>
                    {
                        // query faulted
                        if (tr.IsFaulted || tr.IsCanceled)
                        {
                            return query.NextTry();
                        }

                        // query succeeded
                        return new StarredReposForUser(starrer, tr.Result);
                    }).PipeTo(sender);
            });

            //query all starrers for a repository
            Receive<RetryableQuery>(query => query.Query is QueryStarrers, query =>
            {
                // ReSharper disable once PossibleNullReferenceException (we know from the previous IS statement that this is not null)
                RepoKey starrers = (query.Query as QueryStarrers).Key;

                // close over the Sender in an instance variable
                IActorRef sender = Sender;
                _gitHubClient.Activity.Starring.GetAllStargazers(starrers.Owner, starrers.Repo)
                    .ContinueWith<object>(tr =>
                    {
                        // query faulted
                        if (tr.IsFaulted || tr.IsCanceled)
                        {
                            return query.NextTry();
                        }
                        return tr.Result.ToArray();
                    }).PipeTo(sender);
            });
        }
    }
}
