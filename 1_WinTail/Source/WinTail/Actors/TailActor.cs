﻿using System;
using System.IO;
using System.Text;

using Akka.Actor;

using WinTail.MessageType;


namespace WinTail.Actors
{
    public class TailActor : UntypedActor
    {
        private readonly string _filePath;
        private readonly IActorRef _reporterActor;
        private Stream _fileStream;
        private StreamReader _fileStreamReader;
        private FileObserver _observer;


        public TailActor(IActorRef reporterActor, string filePath)
        {
            _reporterActor = reporterActor;
            _filePath = filePath;
        }


        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                // move file cursor forward
                // pull results from cursor to end of file and write to output
                // (tis is assuming a log file type format that is append-only)
                string text = _fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    _reporterActor.Tell(text);
                }
            }
            else if (message is FileError)
            {
                var fe = message as FileError;
                _reporterActor.Tell(string.Format("Tail error: {0}", fe.Reason));
            }
            else if (message is InitialRead)
            {
                var ir = message as InitialRead;
                _reporterActor.Tell(ir.Text);
            }
        }


        /// <summary>
        /// Cleanup OS handles for <see cref="_fileStreamReader"/> and <see cref="FileObserver"/>.
        /// </summary>
        protected override void PostStop()
        {
            _observer.Dispose();
            _observer = null;
            _fileStreamReader.Close();
            _fileStreamReader.Dispose();
            base.PostStop();
        }


        /// <summary>
        /// Initialization logic for actor that will tail changes to a file.
        /// </summary>
        protected override void PreStart()
        {
            // start watching file for changes
            _observer = new FileObserver(Self, Path.GetFullPath(_filePath));
            _observer.Start();

            // open the file stream with shared read/write permissions (so file can be written to while open)
            _fileStream = new FileStream(Path.GetFullPath(_filePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _fileStreamReader = new StreamReader(_fileStream, Encoding.UTF8);

            // read the initial contents of the file and send it to console as first message
            string text = _fileStreamReader.ReadToEnd();
            Self.Tell(new InitialRead(_filePath, text));
        }
    }
}
