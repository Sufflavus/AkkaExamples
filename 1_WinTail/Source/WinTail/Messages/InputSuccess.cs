using System;


namespace WinTail.Messages
{
    /// <summary>
    ///     Base class for signalling that user input was valid.
    /// </summary>
    public sealed class InputSuccess
    {
        public InputSuccess(string reason)
        {
            Reason = reason;
        }


        public string Reason { get; private set; }
    }
}
