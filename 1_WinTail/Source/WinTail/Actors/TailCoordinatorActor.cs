using System;

using Akka.Actor;

using WinTail.MessageType;


namespace WinTail.Actors
{
    public class TailCoordinatorActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is StartTail)
            {
                var msg = message as StartTail;

                Context.ActorOf(Props.Create(() => new TailActor(msg.ReporterActor, msg.FilePath)));
            }
        }


        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10, // maxNumberOfRetries
                TimeSpan.FromSeconds(30), // duration
                x =>
                {
                    //Maybe we consider ArithmeticException to not be application critical
                    //so we just ignore the error and keep going.
                    if (x is ArithmeticException)
                    {
                        return Directive.Resume;
                    }
                    else if (x is NotSupportedException)
                    {
                        return Directive.Stop; //Error that we cannot recover from, stop the failing actor
                    }
                    else
                    {
                        return Directive.Restart; //In all other cases, just restart the failing actor
                    }
                });
        }
    }
}
