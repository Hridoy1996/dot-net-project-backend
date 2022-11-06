namespace Shared.Models
{
    public class CommonResponseModel
    {
        public int StatusCode { get; set; }
        public dynamic? ResponseData { get; set; }
        public string? ResponseMessage { get; set; }
        public bool IsSucceed { get; set; }
    }
}
