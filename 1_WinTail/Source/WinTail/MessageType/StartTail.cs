using System;

using Akka.Actor;


namespace WinTail.MessageType
{
    /// <summary>
    /// Start tailing the file at user-specified path.
    /// </summary>
    public class StartTail
    {
        public StartTail(string filePath, IActorRef reporterActor)
        {
            FilePath = filePath;
            ReporterActor = reporterActor;
        }


        public string FilePath { get; private set; }

        public IActorRef ReporterActor { get; private set; }
    }
}
