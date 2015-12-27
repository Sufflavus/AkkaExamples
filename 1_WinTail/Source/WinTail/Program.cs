using System;

using Akka.Actor;

using WinTail.Actors;


namespace WinTail
{
    internal class Program
    {
        public static ActorSystem MyActorSystem;


        private static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // make actors
            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, ActorsNames.ConsoleWriterActor);

            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, ActorsNames.TailCoordinatorActor);

            Props validationActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, ActorsNames.ValidationActor);

            Props consoleReaderProps = Props.Create<ConsoleReaderActor>();
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, ActorsNames.ConsoleReaderActor);

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.AwaitTermination();
        }
    }
}
