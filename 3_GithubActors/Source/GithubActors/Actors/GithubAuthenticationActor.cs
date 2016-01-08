using System;
using System.Drawing;

using Akka.Actor;

using GithubActors.Messages.GithubAuthentication;
using GithubActors.Views;

using Octokit;

using Label = System.Windows.Forms.Label;


namespace GithubActors.Actors
{
    public class GithubAuthenticationActor : ReceiveActor
    {
        private readonly GithubAuth _form;
        private readonly Label _statusLabel;


        public GithubAuthenticationActor(Label statusLabel, GithubAuth form)
        {
            _statusLabel = statusLabel;
            _form = form;
            Unauthenticated();
        }


        private void Authenticating()
        {
            Receive<AuthenticationFailed>(failed => BecomeUnauthenticated("Authentication failed."));
            Receive<AuthenticationCancelled>(cancelled => BecomeUnauthenticated("Authentication timed out."));
            Receive<AuthenticationSuccess>(success =>
            {
                var launcherForm = new LauncherForm();
                launcherForm.Show();
                _form.Hide();
            });
        }


        private void BecomeAuthenticating()
        {
            _statusLabel.Visible = true;
            _statusLabel.ForeColor = Color.Yellow;
            _statusLabel.Text = "Authenticating...";
            Become(Authenticating);
        }


        private void BecomeUnauthenticated(string reason)
        {
            _statusLabel.ForeColor = Color.Red;
            _statusLabel.Text = "Authentication failed. Please try again.";
            Become(Unauthenticated);
        }


        private void Unauthenticated()
        {
            Receive<Authenticate>(auth =>
            {
                //need a client to test our credentials with
                GitHubClient client = GithubClientFactory.GetUnauthenticatedClient();
                GithubClientFactory.OAuthToken = auth.OAuthToken;
                client.Credentials = new Credentials(auth.OAuthToken);
                BecomeAuthenticating();
                client.User.Current().ContinueWith<object>(tr =>
                {
                    if (tr.IsFaulted)
                    {
                        return new AuthenticationFailed();
                    }
                    if (tr.IsCanceled)
                    {
                        return new AuthenticationCancelled();
                    }
                    return new AuthenticationSuccess();
                }).PipeTo(Self);
            });
        }
    }
}
