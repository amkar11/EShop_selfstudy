namespace User.Domain.Models.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid login or password") { }
    }
}
