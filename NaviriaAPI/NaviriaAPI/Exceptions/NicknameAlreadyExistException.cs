namespace NaviriaAPI.Exceptions
{
    public class NicknameAlreadyExistException : Exception
    {
        public NicknameAlreadyExistException(string message) : base(message) { }
    }
}
