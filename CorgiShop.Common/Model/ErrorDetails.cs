using System.Text.Json;

namespace CorgiShop.Common.Model;

public record ErrorDetails(int StatusCode, string Details)
{
    public override string ToString()
    {
        /*
         * Errors returned from the API take this JSON form
         * Just serialize the record - StatusCode, Details, and any other future properties
         */
        return JsonSerializer.Serialize(this);
    }
}
