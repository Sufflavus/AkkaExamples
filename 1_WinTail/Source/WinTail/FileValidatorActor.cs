using System;
using System.IO;

using Akka.Actor;

using WinTail.Messages;


namespace WinTail
{
    /// <summary>
    /// Actor that validates user input and signals result to others.
    /// </summary>
    public sealed class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;


        public FileValidatorActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }


        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                // signal that the user needs to supply an input
                _consoleWriterActor.Tell(new NullInputError("No input received."));

                // tell sender to continue doing its thing (whatever that may be, this actor doesn't care)
                Sender.Tell(new ContinueProcessing());
            }
            else
            {
                bool valid = IsFileUri(msg);
                if (valid)
                {
                    // send success to console writer
                    _consoleWriterActor.Tell(new InputSuccess(string.Format("Starting processing for {0}", msg)));

                    // start coordinator
                    Context.ActorSelection("akka://MyActorSystem/user/tailCoordinatorActor")
                        .Tell(new TailCoordinatorActor.StartTail(msg, _consoleWriterActor));
                }
                else
                {
                    // signal that input was bad
                    _consoleWriterActor.Tell(new ValidationError(string.Format("{0} is not an existing URI on disk.", msg)));

                    // tell sender to continue doing its thing (whatever that may be, this actor doesn't care)
                    Sender.Tell(new ContinueProcessing());
                }
            }
        }


        /// <summary>
        /// Checks if file exists at path provided by user.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsFileUri(string path)
        {
            return File.Exists(path);
        }
    }
}
