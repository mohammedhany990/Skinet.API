using Skinet.Core.Helper;
using System.Text.Json.Serialization;

namespace Skinet.API.Errors
{
    public class ExceptionResponse : BaseResponse<string>
    {
        [JsonPropertyOrder(8)]
        public string? Details { get; set; }
        public ExceptionResponse(int code, string? msg = null, string? details = null)
            : base(code, msg)
        {
            Details = details;
        }
    }
}
