using System;

using Akka.Actor;

using WinTail.Messages;


namespace WinTail
{
    /// <summary>
    ///     Actor responsible for reading FROM the console.
    ///     Also responsible for calling <see cref="ActorSystem.Shutdown" />.
    /// </summary>
    internal class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";
        private readonly IActorRef _consoleWriterActor;


        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }


        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is InputError)
            {
                _consoleWriterActor.Tell(message as InputError);
            }

            GetAndValidateInput();
        }


        /// <summary>
        ///     Validates <see cref="message" />.
        ///     Currently says messages are valid if contain even number of characters.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool IsValid(string message)
        {
            bool valid = message.Length % 2 == 0;
            return valid;
        }


        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }


        /// <summary>
        ///     Reads input from console, validates it, then signals appropriate response
        ///     (continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            string message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                // signal that the user needs to supply an input, as previously
                // received input was blank
                Self.Tell(new NullInputError("No input received."));
            }
            else if (String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shut down the entire actor system (allows the process to exit)
                Context.System.Shutdown();
            }
            else
            {
                bool valid = IsValid(message);
                if (valid)
                {
                    _consoleWriterActor.Tell(new InputSuccess("Thank you! Message was valid."));

                    // continue reading messages from console
                    _consoleWriterActor.Tell(new ContinueProcessing());
                }
                else
                {
                    Self.Tell(new ValidationError("Invalid: input had odd number of characters."));
                }
            }
        }
    }
}
