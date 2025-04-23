namespace NaviriaAPI.Exceptions
{
    public class FailedToUpdateException : Exception
    {
        public FailedToUpdateException(string message)
            : base(message)
        {
        }
    }
}
