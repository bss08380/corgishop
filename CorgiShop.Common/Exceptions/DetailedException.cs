using CorgiShop.Common.Model;
using System.Net;

namespace CorgiShop.Common.Exceptions;

public class DetailedException : Exception, IDetailedException
{
    public ErrorDetails Details { get; }

	public DetailedException(string message, HttpStatusCode statusCode, Exception? innerException = null)
		: base(message, innerException)
	{
		Details = new ErrorDetails()
		{
			StatusCode = statusCode,
			Details = message
        };
	}

	public static DetailedException FromUnknownError(Exception ex) => new DetailedException("Unknown Error", HttpStatusCode.InternalServerError, ex);

    public static DetailedException FromDatabaseUpdateError(Exception ex) => new DetailedException("Unknown Database Write Error", HttpStatusCode.InternalServerError, ex);

    public static DetailedException FromFailedVerification(string propName, string failureReason) => new DetailedException($"Verification failed: {propName} -- {failureReason}", HttpStatusCode.BadRequest);

}
