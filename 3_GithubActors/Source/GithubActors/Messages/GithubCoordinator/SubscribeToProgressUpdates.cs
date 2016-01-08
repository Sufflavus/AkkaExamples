using System;

using Akka.Actor;


namespace GithubActors.Messages.GithubCoordinator
{
    public class SubscribeToProgressUpdates
    {
        public SubscribeToProgressUpdates(IActorRef subscriber)
        {
            Subscriber = subscriber;
        }


        public IActorRef Subscriber { get; private set; }
    }
}
