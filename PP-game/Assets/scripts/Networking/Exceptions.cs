using System;
using System.Runtime.Serialization;

public class NotServerException : Exception {
  public NotServerException() {
  }

  public NotServerException(string message) : base(message) {
  }

  public NotServerException(string message, Exception innerException) : base(message, innerException) {
  }

  protected NotServerException(SerializationInfo info, StreamingContext context) : base(info, context) {
  }
}

public class NotClientException : Exception {
  public NotClientException() {
  }

  public NotClientException(string message) : base(message) {
  }

  public NotClientException(string message, Exception innerException) : base(message, innerException) {
  }

  protected NotClientException(SerializationInfo info, StreamingContext context) : base(info, context) {
  }
}