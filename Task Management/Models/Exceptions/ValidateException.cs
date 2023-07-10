namespace Task_Management.Models.Exceptions
{
    public class ValidateException : Exception
    {
        public ValidateException(string message)
            : base(message) { }
    }
}
