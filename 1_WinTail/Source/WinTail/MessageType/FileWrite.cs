using System;


namespace WinTail.MessageType
{
    /// <summary>
    /// Signal that the file has changed, and we need to read the next line of the file.
    /// </summary>
    public class FileWrite
    {
        public FileWrite(string fileName)
        {
            FileName = fileName;
        }


        public string FileName { get; private set; }
    }
}
