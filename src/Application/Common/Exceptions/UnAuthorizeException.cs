namespace Application.Common.Exceptions;

public sealed class UnAuthorizeException(string message) : Exception(message);