using Skinet.Core.Helper;

namespace Skinet.API.Errors
{
    public class ValidationErrorResponse : BaseResponse<string>
    {
        public IEnumerable<string> Errors { get; set; }
        public ValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }


    }
}
