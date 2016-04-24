namespace MiddleMan.Exceptions
{
    public class NoHandlerException : MiddleManExceptionBase
    {
        public NoHandlerException(string message) : base(message)
        {
        }
    }
}