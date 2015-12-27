using System;


namespace WinTail.MessageType
{
    /// <summary>
    /// Signal that the OS had an error accessing the file.
    /// </summary>
    public class FileError
    {
        public FileError(string reason, string fileName)
        {
            Reason = reason;
            FileName = fileName;
        }


        public string FileName { get; private set; }
        public string Reason { get; private set; }
    }
}
