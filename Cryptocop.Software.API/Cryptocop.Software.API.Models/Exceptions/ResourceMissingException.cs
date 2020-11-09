using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
  public class ResourceMissingException : Exception
  {
    public ResourceMissingException() : base("Resource is missing.") { }
    public ResourceMissingException(string message) : base(message) { }
    public ResourceMissingException(string message, Exception inner) : base(message, inner) { }
  }
}