using FluentValidation.Results;
using System.Net;

namespace Domains.HelperModels
{
    public class DynamicQueryResponseHandler
    {
        public string Message { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public dynamic Data { get; private set; }
        public IEnumerable<string> Errors { get; set; }

        public DynamicQueryResponseHandler()
        {
            Errors = new List<string>();
        }

        public DynamicQueryResponseHandler HandleQuerySuccess(dynamic data, HttpStatusCode successStatusCode)
        {
            Data = data;
            HttpStatusCode = successStatusCode;
            return this;
        }

        public DynamicQueryResponseHandler HandleQueryError(HttpStatusCode successStatusCode, string errorMessage, ValidationResult validationError = null)
        {
            Errors = validationError != null ? GetErrorMessages(validationError) : new List<string>();
            Message = errorMessage;
            HttpStatusCode = successStatusCode;

            return this;
        }
        private IEnumerable<string> GetErrorMessages(ValidationResult validationError)
        {
            if (validationError != null)
            {
                foreach (ValidationFailure error in validationError.Errors)
                {
                    yield return error.ErrorMessage;
                }
            }
        }
    }


}
