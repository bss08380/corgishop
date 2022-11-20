using System.Net;
using System.Text.Json;

namespace CorgiShop.Common.Model;

public record ErrorDetails
{
    public required HttpStatusCode StatusCode { get; set; }
    public required string Details { get; set; }

    public override string ToString()
    {
        /*
         * Errors returned from the API take this JSON form
         * Just serialize the record - StatusCode, Details, and any other future properties
         */
        return JsonSerializer.Serialize(this);
    }
}
