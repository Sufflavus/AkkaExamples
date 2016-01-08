using System;


namespace GithubActors.Messages.GithubAuthentication
{
    public class Authenticate
    {
        public Authenticate(string oAuthToken)
        {
            OAuthToken = oAuthToken;
        }


        public string OAuthToken { get; private set; }
    }
}
