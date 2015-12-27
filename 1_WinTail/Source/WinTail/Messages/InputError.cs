using System;


namespace WinTail.Messages
{
    /// <summary>
    ///     Base class for signalling that user input was invalid.
    /// </summary>
    public class InputError
    {
        public InputError(string reason)
        {
            Reason = reason;
        }


        public string Reason { get; private set; }
    }
}
