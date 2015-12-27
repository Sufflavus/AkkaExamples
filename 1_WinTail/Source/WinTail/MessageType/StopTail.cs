using System;


namespace WinTail.MessageType
{
    /// <summary>
    /// Stop tailing the file at user-specified path.
    /// </summary>
    public class StopTail
    {
        public StopTail(string filePath)
        {
            FilePath = filePath;
        }


        public string FilePath { get; private set; }
    }
}
