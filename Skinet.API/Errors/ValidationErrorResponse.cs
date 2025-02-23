using Skinet.Core.Helper;
using System.Text.Json.Serialization;

namespace Skinet.API.Errors
{
    public class ValidationErrorResponse : BaseResponse<object>
    {
        [JsonPropertyOrder(7)]
        public IEnumerable<string> Errors { get; set; }

        public ValidationErrorResponse(IEnumerable<string> errors) : base(400, false)
        {
            Errors = errors;
        }
    }

}
