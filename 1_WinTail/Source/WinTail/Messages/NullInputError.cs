using System;


namespace WinTail.Messages
{
    /// <summary>
    ///     User provided blank input.
    /// </summary>
    public sealed class NullInputError : InputError
    {
        public NullInputError(string reason)
            : base(reason)
        {
        }
    }
}
