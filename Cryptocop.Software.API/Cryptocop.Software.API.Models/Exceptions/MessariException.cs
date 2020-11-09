using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
  public class MessariException : Exception
  {
    public MessariException() : base("An exception was caught from the Messari api.") { }
    public MessariException(string message) : base(message) { }
    public MessariException(string message, Exception inner) : base(message, inner) { }
  }
}