using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
  public class InvalidSignInException : Exception
  {
    public InvalidSignInException() : base("Invalid email or password.") { }
  }
}