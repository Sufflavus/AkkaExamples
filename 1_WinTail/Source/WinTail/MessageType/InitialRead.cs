using System;


namespace WinTail.MessageType
{
    /// <summary>
    /// Signal to read the initial contents of the file at actor startup.
    /// </summary>
    public class InitialRead
    {
        public InitialRead(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }


        public string FileName { get; private set; }
        public string Text { get; private set; }
    }
}
