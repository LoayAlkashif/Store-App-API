namespace Store.APIs.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public ApiErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }


        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = StatusCode switch
            {
                400 => "a bad Request",
                401 => "You are not Authorized",
                404 => "Resource Not Found",
                500=> "Server Error",
                _ => null

            };

            return message;
        }
    }
}
