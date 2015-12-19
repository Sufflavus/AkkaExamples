using System;

using Akka.Actor;

using WinTail.Messages;


namespace WinTail
{
    /// <summary>
    /// Actor that validates user input and signals result to others.
    /// </summary>
    public sealed class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;


        public ValidationActor(IActorRef consoleWriterActor)
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
            }
            else
            {
                bool valid = IsValid(msg);
                if (valid)
                {
                    // send success to console writer
                    _consoleWriterActor.Tell(new InputSuccess("Thank you! Message was valid."));
                }
                else
                {
                    _consoleWriterActor.Tell(new ValidationError("Invalid: input had odd number of characters."));
                }
            }

            Sender.Tell(new ContinueProcessing());
        }


        /// <summary>
        /// Determines if the message received is valid.
        /// Currently, arbitrarily checks if number of chars in message received is even.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool IsValid(string msg)
        {
            bool valid = msg.Length % 2 == 0;
            return valid;
        }
    }
}
