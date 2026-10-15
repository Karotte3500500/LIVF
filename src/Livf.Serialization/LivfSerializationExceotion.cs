namespace Livf.Serialization;

public sealed class LivfSerializationException : Exception
{
    public LivfSerializationErrorCode ErrorCode { get; }

    public LivfSerializationException(LivfSerializationErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public LivfSerializationException(LivfSerializationErrorCode errorCode, string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}