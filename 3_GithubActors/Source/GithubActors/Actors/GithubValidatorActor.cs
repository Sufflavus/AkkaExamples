using System;
using System.Collections.Generic;
using System.Linq;

using Akka.Actor;

using GithubActors.Data;
using GithubActors.Messages.GithubCommander;
using GithubActors.Messages.GithubValidator;

using Octokit;


namespace GithubActors.Actors
{
    /// <summary>
    /// Actor has one job - ensure that a public repo exists at the specified address
    /// </summary>
    public class GithubValidatorActor : ReceiveActor
    {
        private readonly IGitHubClient _gitHubClient;


        public GithubValidatorActor(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
            ReadyToValidate();
        }


        public static Tuple<string, string> SplitIntoOwnerAndRepo(string repoUri)
        {
            List<string> split = new Uri(repoUri, UriKind.Absolute).PathAndQuery.TrimEnd('/').Split('/').Reverse().ToList(); //uri path without trailing slash
            return Tuple.Create(split[1], split[0]); //User, Repo
        }


        private void ReadyToValidate()
        {
            //Outright invalid URLs
            Receive<ValidateRepo>(repo => string.IsNullOrEmpty(repo.RepoUri) || !Uri.IsWellFormedUriString(repo.RepoUri, UriKind.Absolute),
                repo => Sender.Tell(new InvalidRepo(repo.RepoUri, "Not a valid absolute URI")));

            //Repos that at least have a valid absolute URL
            Receive<ValidateRepo>(repo =>
            {
                Tuple<string, string> userOwner = SplitIntoOwnerAndRepo(repo.RepoUri);
                //close over the sender in an instance variable
                IActorRef sender = Sender;
                _gitHubClient.Repository.Get(userOwner.Item1, userOwner.Item2).ContinueWith<object>(t =>
                {
                    //Rule #1 of async in Akka.NET - turn exceptions into messages your actor understands
                    if (t.IsCanceled)
                    {
                        return new InvalidRepo(repo.RepoUri, "Repo lookup timed out");
                    }
                    if (t.IsFaulted)
                    {
                        return new InvalidRepo(repo.RepoUri, t.Exception != null ? t.Exception.GetBaseException().Message : "Unknown Octokit error");
                    }

                    return t.Result;
                }).PipeTo(Self, sender);
            });

            // something went wrong while querying github, sent to ourselves via PipeTo
            // however - Sender gets preserved on the call, so it's safe to use Forward here.
            Receive<InvalidRepo>(repo => Sender.Forward(repo));

            // Octokit was able to retrieve this repository
            Receive<Repository>(repository =>
                Context.ActorSelection(ActorPaths.GithubCommanderActor.Path)
                    .Tell(new CanAcceptJob(new RepoKey(repository.Owner.Login, repository.Name))));

            /* REPO is valid, but can we process it at this time? */

            //yes
            Receive<UnableToAcceptJob>(job => Context.ActorSelection(ActorPaths.MainFormActor.Path).Tell(job));

            //no
            Receive<AbleToAcceptJob>(job => Context.ActorSelection(ActorPaths.MainFormActor.Path).Tell(job));
        }
    }
}
