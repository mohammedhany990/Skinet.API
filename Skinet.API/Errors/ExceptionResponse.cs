using Skinet.Core.Helper;

namespace Skinet.API.Errors
{
    public class ExceptionResponse : BaseResponse<string>
    {
        public string? Details { get; set; }
        public ExceptionResponse(int code, string? msg = null, string? details = null)
            : base(code, msg)
        {
            Details = details;
        }
    }
}
