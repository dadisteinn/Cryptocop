using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
  public class InvalidRegistrationException : Exception
  {
    public InvalidRegistrationException() : base("A user already exists with this email.") { }
  }
}