using System;


namespace WinTail.Messages
{
    /// <summary>
    ///     User provided invalid input (currently, input w/ odd # chars)
    /// </summary>
    public sealed class ValidationError : InputError
    {
        public ValidationError(string reason)
            : base(reason)
        {
        }
    }
}
