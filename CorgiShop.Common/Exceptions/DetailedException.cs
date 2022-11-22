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

	public static DetailedException FromUnknownError(Exception ex)
	{
		/*
		 * In a real world application, I would not typically return ex.Message raw out to the user
		 * It helps for debugging though, given that this is a learning application
		 */
        return new DetailedException(ex.Message /* "Unknown Error" */, HttpStatusCode.InternalServerError, ex);
    }

    public static DetailedException FromDatabaseUpdateError(Exception ex) => new DetailedException("Unknown Database Write Error", HttpStatusCode.InternalServerError, ex);

    public static DetailedException FromFailedVerification(string propName, string failureReason) => new DetailedException($"Verification failed: {propName} -- {failureReason}", HttpStatusCode.BadRequest);

}
