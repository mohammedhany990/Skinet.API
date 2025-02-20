using System.Text.Json.Serialization;

namespace Skinet.API.Errors
{
    public class ApiResponse
    {
        [JsonPropertyOrder(1)]
        public int StatusCode { get; set; }
        [JsonPropertyOrder(2)]
        public string Message { get; set; }
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMsg(statusCode);
        }

        private string? GetDefaultMsg(int code)
        {
            return code switch
            {
                200 => "OK",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                408 => "Request Timeout",
                409 => "Conflict",
                429 => "Too Many Requests",
                500 => "Internal Server Error",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                _ => null
            };
        }

    }
}
