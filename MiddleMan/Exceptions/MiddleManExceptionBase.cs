namespace MiddleMan.Exceptions
{
    using System;

    public class MiddleManExceptionBase : Exception
    {
        public MiddleManExceptionBase(string message) : base(message)
        {
        }
    }
}