using System;


namespace GithubActors.Messages.GithubCoordinator
{
    public class PublishUpdate
    {
        private static readonly PublishUpdate _instance = new PublishUpdate();


        private PublishUpdate()
        {
        }


        public static PublishUpdate Instance
        {
            get { return _instance; }
        }
    }
}
