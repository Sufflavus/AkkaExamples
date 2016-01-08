using System;

using Akka.Actor;

using GithubActors.Data;


namespace GithubActors.Messages.MainForm
{
    public class LaunchRepoResultsWindow
    {
        public LaunchRepoResultsWindow(RepoKey repo, IActorRef coordinator)
        {
            Repo = repo;
            Coordinator = coordinator;
        }


        public IActorRef Coordinator { get; private set; }
        public RepoKey Repo { get; private set; }
    }
}
