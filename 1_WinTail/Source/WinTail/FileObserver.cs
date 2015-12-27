using System;
using System.IO;

using Akka.Actor;

using WinTail.Actors;
using WinTail.MessageType;


namespace WinTail
{
    /// <summary>
    /// Turns <see cref="FileSystemWatcher"/> events about a specific file into messages for <see cref="TailActor"/>.
    /// </summary>
    public sealed class FileObserver : IDisposable
    {
        private readonly string _absoluteFilePath;
        private readonly string _fileDir;
        private readonly string _fileNameOnly;
        private readonly IActorRef _tailActor;
        private FileSystemWatcher _watcher;


        public FileObserver(IActorRef tailActor, string absoluteFilePath)
        {
            _tailActor = tailActor;
            _absoluteFilePath = absoluteFilePath;
            _fileDir = Path.GetDirectoryName(absoluteFilePath);
            _fileNameOnly = Path.GetFileName(absoluteFilePath);
        }


        /// <summary>
        /// Begin monitoring file.
        /// </summary>
        public void Start()
        {
            // make watcher to observe our specific file
            _watcher = new FileSystemWatcher(_fileDir, _fileNameOnly);

            // watch our file for changes to the file name, or new messages being written to file
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            // assign callbacks for event types
            _watcher.Changed += OnFileChanged;
            _watcher.Error += OnFileError;

            // start watching
            _watcher.EnableRaisingEvents = true;
        }


        public void Dispose()
        {
            _watcher.Dispose();
        }


        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                _tailActor.Tell(new FileWrite(e.Name), ActorRefs.NoSender);
            }
        }


        /// <summary>
        /// Callback for <see cref="FileSystemWatcher"/> file error events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFileError(object sender, ErrorEventArgs e)
        {
            _tailActor.Tell(new FileError(_fileNameOnly, e.GetException().Message), ActorRefs.NoSender);
        }
    }
}
